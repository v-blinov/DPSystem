﻿// <auto-generated />
using System;
using ApiDPSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ApiDPSystem.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20210804115850_AddNessessaryFields")]
    partial class AddNessessaryFields
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ApiDPSystem.Entities.CarEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ConfigurationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DealerId")
                        .HasColumnType("int");

                    b.Property<int>("ExteriorColorId")
                        .HasColumnType("int");

                    b.Property<int>("InteriorColorId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAvailable")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("VinCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("ConfigurationId");

                    b.HasIndex("DealerId");

                    b.HasIndex("ExteriorColorId");

                    b.HasIndex("InteriorColorId");

                    b.ToTable("CarEntities");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.CarImage", b =>
                {
                    b.Property<Guid>("CarEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ImageId")
                        .HasColumnType("int");

                    b.HasKey("CarEntityId", "ImageId");

                    b.HasIndex("ImageId");

                    b.ToTable("CarImages");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.Color", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("HexCode")
                        .HasMaxLength(6)
                        .HasColumnType("varchar(6)");

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.Configuration", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Drive")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("EngineId")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ModelTrim")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("ProducerId")
                        .HasColumnType("int");

                    b.Property<string>("Transmission")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<short>("Year")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("EngineId");

                    b.HasIndex("ProducerId");

                    b.ToTable("Configurations");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.ConfigurationFeature", b =>
                {
                    b.Property<Guid>("ConfigurationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("FeatureId")
                        .HasColumnType("int");

                    b.HasKey("ConfigurationId", "FeatureId");

                    b.HasIndex("FeatureId");

                    b.ToTable("ConfigurationFeatures");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.Dealer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.ToTable("Dealers");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.Engine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double?>("Capacity")
                        .HasColumnType("float");

                    b.Property<string>("Fuel")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int?>("Power")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Engines");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.Feature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.Producer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Producers");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.RefreshTokenInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("JwtId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokenInfoTable");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.SoldCar", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ConfigurationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DealerId")
                        .HasColumnType("int");

                    b.Property<int>("ExteriorColorId")
                        .HasColumnType("int");

                    b.Property<int>("InteriorColorId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAvailable")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("VinCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("ConfigurationId");

                    b.HasIndex("DealerId");

                    b.HasIndex("ExteriorColorId");

                    b.HasIndex("InteriorColorId");

                    b.ToTable("SoldCars");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.SoldCarImage", b =>
                {
                    b.Property<Guid>("SoldCarId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ImageId")
                        .HasColumnType("int");

                    b.HasKey("SoldCarId", "ImageId");

                    b.HasIndex("ImageId");

                    b.ToTable("SoldCarImages");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.CarEntity", b =>
                {
                    b.HasOne("ApiDPSystem.Entities.Configuration", "Configuration")
                        .WithMany("CarEntities")
                        .HasForeignKey("ConfigurationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiDPSystem.Entities.Dealer", "Dealer")
                        .WithMany("CarEntities")
                        .HasForeignKey("DealerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiDPSystem.Entities.Color", "ExteriorColor")
                        .WithMany("ExteriorCarEntity")
                        .HasForeignKey("ExteriorColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiDPSystem.Entities.Color", "InteriorColor")
                        .WithMany("InteriorCarEntity")
                        .HasForeignKey("InteriorColorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Configuration");

                    b.Navigation("Dealer");

                    b.Navigation("ExteriorColor");

                    b.Navigation("InteriorColor");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.CarImage", b =>
                {
                    b.HasOne("ApiDPSystem.Entities.CarEntity", "CarEntity")
                        .WithMany("CarImages")
                        .HasForeignKey("CarEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiDPSystem.Entities.Image", "Image")
                        .WithMany("CarImages")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CarEntity");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.Configuration", b =>
                {
                    b.HasOne("ApiDPSystem.Entities.Engine", "Engine")
                        .WithMany("Configurations")
                        .HasForeignKey("EngineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiDPSystem.Entities.Producer", "Producer")
                        .WithMany("Configurations")
                        .HasForeignKey("ProducerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Engine");

                    b.Navigation("Producer");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.ConfigurationFeature", b =>
                {
                    b.HasOne("ApiDPSystem.Entities.Configuration", "Configuration")
                        .WithMany("ConfigurationFeatures")
                        .HasForeignKey("ConfigurationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiDPSystem.Entities.Feature", "Feature")
                        .WithMany("ConfigurationFeature")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Configuration");

                    b.Navigation("Feature");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.SoldCar", b =>
                {
                    b.HasOne("ApiDPSystem.Entities.Configuration", "Configuration")
                        .WithMany("SoldCars")
                        .HasForeignKey("ConfigurationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiDPSystem.Entities.Dealer", "Dealer")
                        .WithMany("SoldCars")
                        .HasForeignKey("DealerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiDPSystem.Entities.Color", "ExteriorColor")
                        .WithMany("ExteriorSoldCar")
                        .HasForeignKey("ExteriorColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiDPSystem.Entities.Color", "InteriorColor")
                        .WithMany("InteriorSoldCar")
                        .HasForeignKey("InteriorColorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Configuration");

                    b.Navigation("Dealer");

                    b.Navigation("ExteriorColor");

                    b.Navigation("InteriorColor");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.SoldCarImage", b =>
                {
                    b.HasOne("ApiDPSystem.Entities.Image", "Image")
                        .WithMany("SoldCarImages")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiDPSystem.Entities.SoldCar", "SoldCar")
                        .WithMany("SoldCarImages")
                        .HasForeignKey("SoldCarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("SoldCar");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.CarEntity", b =>
                {
                    b.Navigation("CarImages");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.Color", b =>
                {
                    b.Navigation("ExteriorCarEntity");

                    b.Navigation("ExteriorSoldCar");

                    b.Navigation("InteriorCarEntity");

                    b.Navigation("InteriorSoldCar");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.Configuration", b =>
                {
                    b.Navigation("CarEntities");

                    b.Navigation("ConfigurationFeatures");

                    b.Navigation("SoldCars");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.Dealer", b =>
                {
                    b.Navigation("CarEntities");

                    b.Navigation("SoldCars");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.Engine", b =>
                {
                    b.Navigation("Configurations");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.Feature", b =>
                {
                    b.Navigation("ConfigurationFeature");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.Image", b =>
                {
                    b.Navigation("CarImages");

                    b.Navigation("SoldCarImages");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.Producer", b =>
                {
                    b.Navigation("Configurations");
                });

            modelBuilder.Entity("ApiDPSystem.Entities.SoldCar", b =>
                {
                    b.Navigation("SoldCarImages");
                });
#pragma warning restore 612, 618
        }
    }
}