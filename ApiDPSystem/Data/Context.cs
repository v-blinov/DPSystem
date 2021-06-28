using ApiDPSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiDPSystem.Data
{
    public class Context : DbContext
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public Context() : this(DefaultOptions())
        {
        }
        public Context(string connectionString) : this(DefaultOptions(connectionString))
        {
        }
        public Context(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public static DbContextOptions DefaultOptions(string connectionString = null)
        {
            var cs = connectionString ?? DefaultConnection.DefaultConnectionString;
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), cs).Options;
        }
    }
}
