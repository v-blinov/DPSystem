using ApiDPSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDPSystem.Data
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }


        public Context() : this(DefaultOptions())
        {
        }
        public Context(string connectionString) : this(DefaultOptions(connectionString))
        {
        }
        public Context(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
            //SeedData.Initialize(this);
        }
        public static DbContextOptions DefaultOptions(string connectionString = null)
        {
            var cs = connectionString ?? DefaultConnection.DefaultConnectionString;
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), cs).Options;
        }
    }
}
