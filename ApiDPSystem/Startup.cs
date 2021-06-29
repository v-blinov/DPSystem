using ApiDPSystem.Data;
using ApiDPSystem.Models;
using ApiDPSystem.Services;
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
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiDPSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.AddDbContext<Context>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
            })
                .AddGoogle(googleOptions =>
                {
                    IConfigurationSection googleAuthNSection =
                        Configuration.GetSection("Authentication:Google");

                    googleOptions.ClientId = googleAuthNSection["ClientId"];
                    googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];

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

            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });

            services.Configure<DataProtectionTokenProviderOptions>(p => p.TokenLifespan = TimeSpan.FromMinutes(30));

            services.AddSwaggerGen(c =>
            {
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DPSystemAPI", Version = "v1" });

                // addOAuthAuthentication
                var OauthSecurityScheme = new OpenApiSecurityScheme
                {
                    Description = "EnterClientID",
                    Type = SecuritySchemeType.OAuth2,
                    Scheme = "oauth2",
                    Name = "oauth2",
                    In = ParameterLocation.Header,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://accounts.google.com/o/oauth2/auth?" +
                                                        "scope=openid%20profile%20email" +
                                                        "&flowName=GeneralOAuthFlow"),
                            TokenUrl = new Uri(Configuration["OAuth:TokenUrlEndpoint"])
                        }
                    },
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "oauth2"
                    }
                };
                c.AddSecurityDefinition(OauthSecurityScheme.Reference.Id, OauthSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {{ OauthSecurityScheme, new List<string>() }});

                // add JWT Authentication
                var JwtSecurityScheme = new OpenApiSecurityScheme
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
                c.AddSecurityDefinition(JwtSecurityScheme.Reference.Id, JwtSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement{{JwtSecurityScheme, new List<string>() }});
            });

            services.AddSingleton<TokenValidationParameters>();
            services.AddScoped<AccountService>();
            services.AddScoped<EmailService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => 
            { 
                c.SwaggerEndpoint("swagger/v1/swagger.json", "ApiDPSystem v1"); 
                c.RoutePrefix = string.Empty;
                c.OAuthClientId(Configuration.GetValue<string>("Authentication:Google:ClientId"));
                c.OAuthClientSecret(Configuration.GetValue<string>("Authentication:Google:ClientSecret"));
                c.OAuthAppName("OAuth-dpsystem");
                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
