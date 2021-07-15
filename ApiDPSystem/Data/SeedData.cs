using ApiDPSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDPSystem.Data
{
    //public static class SeedData
    //{
    //    public static void Initialize(ModelBuilder builder)
    //    {
    //        SeedUsers(builder);
    //        SeedRoles(configuration);
    //    }


    //    //private static async Task SeedUsersAsync(IConfiguration configuration, UserManager<User> userManager)
    //    //{
    //    //    var user = new User()
    //    //    {
    //    //        UserName = configuration.GetValue<string>("SuperAdmin:Name"),
    //    //        Email = configuration.GetValue<string>("SuperAdmin:Email"),
    //    //        EmailConfirmed = true
    //    //    };

    //    //    var password = configuration.GetValue<string>("SuperAdmin:Password");

    //    //    await userManager.CreateAsync(user, password);
    //    //}

    //    private static void SeedUsers(ModelBuilder builder)
    //    {
    //        var user = new User()
    //        {
    //            UserName = configuration.GetValue<string>("SuperAdmin:Name"),
    //            Email = configuration.GetValue<string>("SuperAdmin:Email"),
    //            EmailConfirmed = true
    //        };

    //        PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
    //        passwordHasher.HashPassword(user, "Admin*123");

    //        builder.Entity<User>().HasData(user);
    //    }


    //    private void SeedRoles(ModelBuilder builder)
    //    {
    //        builder.Entity<IdentityRole>().HasData(
    //            new IdentityRole() { Id = "fab4fac1-c546-41de-aebc-a14da6895711", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
    //            new IdentityRole() { Id = "c7b013f0-5201-4317-abd8-c211f91b7330", Name = "HR", ConcurrencyStamp = "2", NormalizedName = "Human Resource" }
    //            );
    //    }

    //    private void SeedUserRoles(ModelBuilder builder)
    //    {
    //        builder.Entity<IdentityUserRole<string>>().HasData(
    //            new IdentityUserRole<string>() { RoleId = "fab4fac1-c546-41de-aebc-a14da6895711", UserId = "b74ddd14-6340-4840-95c2-db12554843e5" }
    //            );
    //    }


    //}
}
