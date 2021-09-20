using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using ApiDPSystem.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ApiDPSystem.Data
{
    public static class IdentityPreparation
    {
        public static void PreparationUserAccounts(IApplicationBuilder app)
        {
            try
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    SeedData(serviceScope.ServiceProvider.GetService<IdentityContext>(), serviceScope.ServiceProvider.GetService<UserManager<User>>())
                        .GetAwaiter()
                        .GetResult();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Ошибка при заполнении тестовых данных для пользователя", ex);
            }
        }

        private static async Task SeedData(IdentityContext identityContext, UserManager<User> userManager)
        {
            if (identityContext.Users.Any())
            {
                Console.WriteLine("--> We already have users data");
                return;
            }

            Console.WriteLine("--> Seeding users data...");

            var user = new User
            {
                Email = "Blinov.V.A@mail.ru",
                UserName = "blinov.v.a@mail.ru",
                FirstName = "Vitaly",
                LastName = "Blinov",
                EmailConfirmed = true
            };
            var userPassword = "Qwerty123!";
            var userRole = "User";

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await userManager.CreateAsync(user, userPassword);
                await userManager.AddToRoleAsync(user, userRole);
                transaction.Complete();
            }
        }
    }
}