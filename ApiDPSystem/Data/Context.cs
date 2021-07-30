﻿using ApiDPSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiDPSystem.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<RefreshTokenInfo> RefreshTokenInfoTable { get; set; }
        public DbSet<Entities.CarConfiguration> Cars { get; set; }
        public DbSet<ConfigurationFeature> CarFeatures { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Engine> Engines { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<FeatureType> FeatureTypes { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Transmission> Transmissions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CarConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenInfoConfiguration());
            modelBuilder.ApplyConfiguration(new ColorConfiguration());


            modelBuilder.Entity<ConfigurationFeature>()
                .HasKey(p => new { p.CarId, p.FeatureId });

            modelBuilder.Entity<Engine>()
                .Property(p => p.Fuel)
                .HasMaxLength(20);

            modelBuilder.Entity<FeatureType>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Image>()
                .Property(p => p.Url)
                .IsRequired();

            modelBuilder.Entity<Producer>()
                .Property(p => p.Name)
                .HasMaxLength(250)
                .IsRequired();

            modelBuilder.Entity<Transmission>()
                .Property(p => p.Value)
                .HasMaxLength(200)
                .IsRequired();
        }

        public class CarConfiguration : IEntityTypeConfiguration<Entities.CarConfiguration>
        {
            public void Configure(EntityTypeBuilder<Entities.CarConfiguration> builder)
            {
                builder.HasOne(s => s.ExteriorColor)
                    .WithMany(x => x.ExteriorConfiguration)
                    .HasForeignKey(s => s.ExteriorColorId);

                builder.HasOne(s => s.InteriorColor)
                    .WithMany(x => x.InteriorConfiguration)
                    .HasForeignKey(s => s.InteriorColorId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.Property(p => p.VinCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnType("varchar");
                
                builder.Property(p => p.Model)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(p => p.ModelTrim)
                    .IsRequired()
                    .HasMaxLength(20);

                builder.Property(p => p.Year)
                    .HasColumnType("smallint");

                builder.Property(p => p.Drive)
                    .HasMaxLength(10);
            }
        }
        public class ColorConfiguration : IEntityTypeConfiguration<Color>
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
        public class RefreshTokenInfoConfiguration : IEntityTypeConfiguration<RefreshTokenInfo>
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