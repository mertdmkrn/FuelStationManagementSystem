﻿// <auto-generated />
using System;
using FuelStationManagementSystem.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FuelStationManagementSystem.Migrations
{
    [DbContext(typeof(FuelStationDbContext))]
    partial class FuelStationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FuelStationManagementSystem.Models.Balance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("CustomerTCKN")
                        .IsRequired()
                        .HasColumnType("nvarchar(11)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerTCKN");

                    b.ToTable("Balance");
                });

            modelBuilder.Entity("FuelStationManagementSystem.Models.Customer", b =>
                {
                    b.Property<string>("TCKN")
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("NameSurname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("TCKN");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("FuelStationManagementSystem.Models.FuelTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("VehiclePlate")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("VehiclePlate");

                    b.ToTable("FuelTransaction");
                });

            modelBuilder.Entity("FuelStationManagementSystem.Models.Vehicle", b =>
                {
                    b.Property<string>("Plate")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("CustomerTCKN")
                        .IsRequired()
                        .HasColumnType("nvarchar(11)");

                    b.Property<int>("FuelType")
                        .HasColumnType("int");

                    b.Property<int>("VehicleType")
                        .HasColumnType("int");

                    b.HasKey("Plate");

                    b.HasIndex("CustomerTCKN");

                    b.ToTable("Vehicle");
                });

            modelBuilder.Entity("FuelStationManagementSystem.Models.Balance", b =>
                {
                    b.HasOne("FuelStationManagementSystem.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerTCKN")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("FuelStationManagementSystem.Models.FuelTransaction", b =>
                {
                    b.HasOne("FuelStationManagementSystem.Models.Vehicle", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehiclePlate")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("FuelStationManagementSystem.Models.Vehicle", b =>
                {
                    b.HasOne("FuelStationManagementSystem.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerTCKN")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });
#pragma warning restore 612, 618
        }
    }
}
