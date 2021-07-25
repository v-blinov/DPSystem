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
    [Migration("20210723132147_AddDriveFieldInCarTable")]
    partial class AddDriveFieldInCarTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ApiDPSystem.DbEntities.Car", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("CarColorId")
                        .HasColumnType("int");

                    b.Property<int?>("CarFeatureCarId")
                        .HasColumnType("int");

                    b.Property<int?>("CarFeatureFeatureId")
                        .HasColumnType("int");

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

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProducerId")
                        .HasColumnType("int");

                    b.Property<int>("TransmissionId")
                        .HasColumnType("int");

                    b.Property<string>("VinCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<short>("Year")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("CarColorId");

                    b.HasIndex("EngineId");

                    b.HasIndex("ProducerId");

                    b.HasIndex("TransmissionId");

                    b.HasIndex("CarFeatureCarId", "CarFeatureFeatureId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.CarColor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<int>("ColorExteriorId")
                        .HasColumnType("int");

                    b.Property<int?>("ColorId")
                        .HasColumnType("int");

                    b.Property<int>("ColorInteriorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ColorId");

                    b.ToTable("CarColors");
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.CarFeature", b =>
                {
                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<int>("FeatureId")
                        .HasColumnType("int");

                    b.HasKey("CarId", "FeatureId");

                    b.ToTable("CarFeatures");
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.Color", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("HexCode")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("varchar(6)");

                    b.HasKey("Id");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.Engine", b =>
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

            modelBuilder.Entity("ApiDPSystem.DbEntities.Feature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CarFeatureCarId")
                        .HasColumnType("int");

                    b.Property<int?>("CarFeatureFeatureId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FeatureTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CarFeatureCarId", "CarFeatureFeatureId");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.FeatureType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("FeatureId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("FeatureId");

                    b.ToTable("FeatureTypes");
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CarColorId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CarColorId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.Producer", b =>
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

            modelBuilder.Entity("ApiDPSystem.DbEntities.RefreshTokenInfo", b =>
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

            modelBuilder.Entity("ApiDPSystem.DbEntities.Transmission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Transmissions");
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.Car", b =>
                {
                    b.HasOne("ApiDPSystem.DbEntities.CarColor", "CarColor")
                        .WithMany("Cars")
                        .HasForeignKey("CarColorId");

                    b.HasOne("ApiDPSystem.DbEntities.Engine", "Engine")
                        .WithMany("Cars")
                        .HasForeignKey("EngineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiDPSystem.DbEntities.Producer", "Producer")
                        .WithMany("Cars")
                        .HasForeignKey("ProducerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiDPSystem.DbEntities.Transmission", "Transmission")
                        .WithMany("Cars")
                        .HasForeignKey("TransmissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiDPSystem.DbEntities.CarFeature", "CarFeature")
                        .WithMany("Cars")
                        .HasForeignKey("CarFeatureCarId", "CarFeatureFeatureId");

                    b.Navigation("CarColor");

                    b.Navigation("CarFeature");

                    b.Navigation("Engine");

                    b.Navigation("Producer");

                    b.Navigation("Transmission");
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.CarColor", b =>
                {
                    b.HasOne("ApiDPSystem.DbEntities.Color", null)
                        .WithMany("CarColors")
                        .HasForeignKey("ColorId");
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.Feature", b =>
                {
                    b.HasOne("ApiDPSystem.DbEntities.CarFeature", "CarFeature")
                        .WithMany("Features")
                        .HasForeignKey("CarFeatureCarId", "CarFeatureFeatureId");

                    b.Navigation("CarFeature");
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.FeatureType", b =>
                {
                    b.HasOne("ApiDPSystem.DbEntities.Feature", "Feature")
                        .WithMany("FeatureTypes")
                        .HasForeignKey("FeatureId");

                    b.Navigation("Feature");
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.Image", b =>
                {
                    b.HasOne("ApiDPSystem.DbEntities.CarColor", null)
                        .WithMany("Images")
                        .HasForeignKey("CarColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.CarColor", b =>
                {
                    b.Navigation("Cars");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.CarFeature", b =>
                {
                    b.Navigation("Cars");

                    b.Navigation("Features");
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.Color", b =>
                {
                    b.Navigation("CarColors");
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.Engine", b =>
                {
                    b.Navigation("Cars");
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.Feature", b =>
                {
                    b.Navigation("FeatureTypes");
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.Producer", b =>
                {
                    b.Navigation("Cars");
                });

            modelBuilder.Entity("ApiDPSystem.DbEntities.Transmission", b =>
                {
                    b.Navigation("Cars");
                });
#pragma warning restore 612, 618
        }
    }
}
