using ApiDPSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;

namespace ApiDPSystem.Data
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);


        public Context(DbContextOptions<Context> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<RefreshTokenInfo> RefreshTokenInfoTable { get; set; }
        public DbSet<CarEntity> CarEntities { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<ConfigurationFeature> ConfigurationFeatures { get; set; }
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<Engine> Engines { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<SoldCar> SoldCars { get; set; }
        public DbSet<SoldCarImage> SoldCarImages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RefreshTokenInfoSettings());
            modelBuilder.ApplyConfiguration(new ConfigurationSettings());
            modelBuilder.ApplyConfiguration(new CarEntitySettings());
            modelBuilder.ApplyConfiguration(new SoldCarSettings());
            modelBuilder.ApplyConfiguration(new ColorSettings());
            modelBuilder.ApplyConfiguration(new FeatureSettings());


            modelBuilder.Entity<ConfigurationFeature>()
                .HasKey(p => new { p.ConfigurationId, p.FeatureId });

            modelBuilder.Entity<CarImage>()
                .HasKey(p => new { p.CarEntityId, p.ImageId });

            modelBuilder.Entity<SoldCarImage>()
               .HasKey(p => new { p.SoldCarId, p.ImageId });

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

        public class ConfigurationSettings : IEntityTypeConfiguration<Entities.Configuration>
        {
            public void Configure(EntityTypeBuilder<Entities.Configuration> builder)
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
        public class CarEntitySettings : IEntityTypeConfiguration<CarEntity>
        {
            public void Configure(EntityTypeBuilder<CarEntity> builder)
            {
                builder.Property(p => p.VinCode)
                       .IsRequired()
                       .HasMaxLength(20)
                       .HasColumnType("varchar");

                builder.HasOne(s => s.ExteriorColor)
                       .WithMany(x => x.ExteriorCarEntity)
                       .HasForeignKey(s => s.ExteriorColorId);

                builder.HasOne(s => s.InteriorColor)
                       .WithMany(x => x.InteriorCarEntity)
                       .HasForeignKey(s => s.InteriorColorId)
                       .OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class SoldCarSettings : IEntityTypeConfiguration<SoldCar>
        {
            public void Configure(EntityTypeBuilder<SoldCar> builder)
            {
                builder.Property(p => p.VinCode)
                       .IsRequired()
                       .HasMaxLength(20)
                       .HasColumnType("varchar");

                builder.HasOne(s => s.ExteriorColor)
                       .WithMany(x => x.ExteriorSoldCar)
                       .HasForeignKey(s => s.ExteriorColorId);

                builder.HasOne(s => s.InteriorColor)
                       .WithMany(x => x.InteriorSoldCar)
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