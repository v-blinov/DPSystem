using ApiDPSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDPSystem.Data
{
    public static class SeedData
    {
        public static void Initialize(Context db)
        {
            if (!db.Users.Any())
            {
                var user1 = new User { Name = "Ivan" };

                db.Users.AddRange(user1);
                db.SaveChanges();
            }
        }
    }
}
