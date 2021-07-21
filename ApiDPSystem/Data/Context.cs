using ApiDPSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiDPSystem.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<RefreshTokenInfo> RefreshTokenInfoTable { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}