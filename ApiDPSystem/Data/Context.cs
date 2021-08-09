using System;
using ApiDPSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;

namespace ApiDPSystem.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<RefreshTokenInfo> RefreshTokenInfoTable { get; set; }
        public DbSet<CarActual> CarActuals { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<ConfigurationFeature> ConfigurationFeatures { get; set; }
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<Engine> Engines { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<CarHistory> CarHistories { get; set; }
        public DbSet<CarHistoryImage> CarHistoryImages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RefreshTokenInfoSettings());
            modelBuilder.ApplyConfiguration(new ConfigurationSettings());
            modelBuilder.ApplyConfiguration(new CarActualSettings());
            modelBuilder.ApplyConfiguration(new CarHistorySettings());
            modelBuilder.ApplyConfiguration(new ColorSettings());
            modelBuilder.ApplyConfiguration(new FeatureSettings());


            modelBuilder.Entity<ConfigurationFeature>()
                .HasKey(p => new {p.ConfigurationId, p.FeatureId});

            modelBuilder.Entity<CarImage>()
                .HasKey(p => new {p.CarActualId, p.ImageId});

            modelBuilder.Entity<CarHistoryImage>()
                .HasKey(p => new {p.CarHistoryId, p.ImageId});

            modelBuilder.Entity<Engine>()
                .Property(p => p.Fuel)
                .HasMaxLength(20);

            modelBuilder.Entity<Image>()
                .Property(p => p.Url)
                .IsRequired();

            modelBuilder.Entity<Producer>()
                .Property(p => p.Name)
                .HasMaxLength(250)
                .IsRequired();

            modelBuilder.Entity<Feature>()
                .Property(p => p.Description)
                .HasMaxLength(1000)
                .IsRequired();

            modelBuilder.Entity<Dealer>()
                .Property(p => p.Name)
                .HasMaxLength(450)
                .IsRequired();
        }

        public class ConfigurationSettings : IEntityTypeConfiguration<Configuration>
        {
            public void Configure(EntityTypeBuilder<Configuration> builder)
            {
                builder.Property(p => p.Year)
                    .HasColumnType("smallint");

                builder.Property(p => p.Model)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(p => p.ModelTrim)
                    .IsRequired()
                    .HasMaxLength(20);

                builder.Property(p => p.Transmission)
                    .HasMaxLength(10);

                builder.Property(p => p.Drive)
                    .HasMaxLength(10);
            }
        }

        public class CarActualSettings : IEntityTypeConfiguration<CarActual>
        {
            public void Configure(EntityTypeBuilder<CarActual> builder)
            {
                builder.HasIndex(p => new {p.DealerId});

                builder.HasIndex(p => new {p.DealerId, p.Id}).IsUnique();

                builder.Property(p => p.VinCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnType("varchar");

                builder.HasOne(s => s.ExteriorColor)
                    .WithMany(x => x.ExteriorCarActual)
                    .HasForeignKey(s => s.ExteriorColorId);

                builder.HasOne(s => s.InteriorColor)
                    .WithMany(x => x.InteriorCarActual)
                    .HasForeignKey(s => s.InteriorColorId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class CarHistorySettings : IEntityTypeConfiguration<CarHistory>
        {
            public void Configure(EntityTypeBuilder<CarHistory> builder)
            {
                builder.HasIndex(p => new {p.VinCode, p.Version});

                builder.Property(p => p.IsSold)
                    .HasDefaultValue(false);

                builder.Property(p => p.VinCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnType("varchar");

                builder.HasOne(s => s.ExteriorColor)
                    .WithMany(x => x.ExteriorCarHistory)
                    .HasForeignKey(s => s.ExteriorColorId);

                builder.HasOne(s => s.InteriorColor)
                    .WithMany(x => x.InteriorCarHistory)
                    .HasForeignKey(s => s.InteriorColorId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class ColorSettings : IEntityTypeConfiguration<Color>
        {
            public void Configure(EntityTypeBuilder<Color> builder)
            {
                builder.Property(p => p.HexCode)
                    .HasMaxLength(6)
                    .HasColumnType("varchar");

                builder.Property(p => p.Name)
                    .HasMaxLength(30);
            }
        }

        public class FeatureSettings : IEntityTypeConfiguration<Feature>
        {
            public void Configure(EntityTypeBuilder<Feature> builder)
            {
                builder.Property(p => p.Description)
                    .HasMaxLength(1000)
                    .IsRequired();

                builder.Property(p => p.Type)
                    .HasMaxLength(100)
                    .IsRequired();
            }
        }

        public class RefreshTokenInfoSettings : IEntityTypeConfiguration<RefreshTokenInfo>
        {
            public void Configure(EntityTypeBuilder<RefreshTokenInfo> builder)
            {
                builder.Property(p => p.UserId)
                    .HasMaxLength(450)
                    .IsRequired();

                builder.Property(p => p.RefreshToken)
                    .IsRequired();

                builder.Property(p => p.JwtId)
                    .IsRequired();
            }
        }
    }
}