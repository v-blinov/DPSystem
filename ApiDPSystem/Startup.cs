using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using ApiDPSystem.Data;
using ApiDPSystem.Models;
using ApiDPSystem.Repository;
using ApiDPSystem.Repository.Interfaces;
using ApiDPSystem.Services;
using ApiDPSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;

namespace ApiDPSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region DbContexts and Identity
            services.AddDbContext<IdentityContext>(options =>
                                                       options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<IdentityContext>()
                    .AddDefaultTokenProviders();

            services.AddDbContext<Context>(options =>
                                               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            #endregion

            #region AuthenticationShemes
            services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddGoogle(googleOptions =>
                    {
                        //IConfigurationSection googleAuthNSection =
                        //    Configuration.GetSection("Authentication:Google");
                        //googleOptions.ClientId = googleAuthNSection["ClientId"];
                        //googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];

                        //Надо придумать, как вынести эти данные в docker secret
                        googleOptions.ClientId = "1015102078067-mo5ds31rjrtocd7dfk4vt663946ijftq.apps.googleusercontent.com";
                        googleOptions.ClientSecret = "19-tLf4MHfV13WoYlUN_HXNF";

                        googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
                    })
                    .AddJwtBearer(jwtOptions =>
                    {
                        jwtOptions.SaveToken = true;
                        jwtOptions.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                            ClockSkew = TimeSpan.Zero
                        };
                    });
            #endregion

            #region RabbitMQ
            var rabbitHostName = Environment.GetEnvironmentVariable("RABBIT_HOSTNAME");
            var connectionFactory = new ConnectionFactory
            {
                HostName = rabbitHostName ?? "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            var rabbitMqConnection = connectionFactory.CreateConnection();

            services.AddSingleton(rabbitMqConnection);
            services.AddSingleton<RabbitMqService>();
            #endregion

            services.AddControllers()
                    .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });

            services.Configure<DataProtectionTokenProviderOptions>(p => p.TokenLifespan = TimeSpan.FromMinutes(30));

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                //SwaggerXmlComments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DPSystemAPI", Version = "v1" });

                #region OAuthAuthentication
                // addOAuthAuthentication
                //var OauthSecurityScheme = new OpenApiSecurityScheme
                //{
                //    Description = "EnterClientID",
                //    Type = SecuritySchemeType.OAuth2,
                //    Scheme = "oauth2",
                //    Name = "oauth2",
                //    In = ParameterLocation.Header,
                //    Flows = new OpenApiOAuthFlows
                //    {
                //        Implicit = new OpenApiOAuthFlow
                //        {
                //            AuthorizationUrl = new Uri("https://accounts.google.com/o/oauth2/auth?" +
                //                                        "scope=openid%20profile%20email" +
                //                                        "&flowName=GeneralOAuthFlow"),
                //            TokenUrl = new Uri(Configuration["OAuth:TokenUrlEndpoint"])
                //        }
                //    },
                //    Reference = new OpenApiReference
                //    {
                //        Type = ReferenceType.SecurityScheme,
                //        Id = "oauth2"
                //    }
                //};
                //c.AddSecurityDefinition(OauthSecurityScheme.Reference.Id, OauthSecurityScheme);
                //c.AddSecurityRequirement(new OpenApiSecurityRequirement { { OauthSecurityScheme, new List<string>() } });
                #endregion

                // add JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, new List<string>() } });
            });
            #endregion

            #region AuthorizationByRoles
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", builder => { builder.RequireRole("Admin"); });

                options.AddPolicy("User", builder => { builder.RequireRole("User"); });
            });
            #endregion

            #region DI
            services.AddSingleton<TokenValidationParameters>();
            services.AddScoped<AccountService>();
            services.AddScoped<EmailService>();
            services.AddScoped<RoleService>();
            services.AddScoped<UserService>();
            services.AddScoped<FileService>();
            services.AddScoped<IDataCheckerService, DataCheckerService>();
            services.AddScoped<AccountRepository>();
            services.AddScoped<ICarRepository, CarRepository>();
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("swagger/v1/swagger.json", "ApiDPSystem v1");
                c.RoutePrefix = string.Empty;

                //c.OAuthClientId(Configuration.GetValue<string>("Authentication:Google:ClientId"));
                //c.OAuthClientSecret(Configuration.GetValue<string>("Authentication:Google:ClientSecret"));
                //c.OAuthAppName("OAuth-dpsystem");
                //c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            IdentityPreparation.PreparationUserAccounts(app);
            ContextPreparation.PreparationCarsInfo(app);
        }
    }
}