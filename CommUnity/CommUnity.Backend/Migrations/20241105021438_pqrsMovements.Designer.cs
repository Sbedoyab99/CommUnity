﻿// <auto-generated />
using System;
using CommUnity.BackEnd.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CommUnity.BackEnd.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241105021438_pqrsMovements")]
    partial class pqrsMovements
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CommUnity.Shared.Entities.Apartment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ResidentialUnitId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ResidentialUnitId", "Number")
                        .IsUnique();

                    b.ToTable("Apartments");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("StateId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StateId", "Name")
                        .IsUnique();

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.CommonZone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ResidentialUnitId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ResidentialUnitId", "Name")
                        .IsUnique();

                    b.ToTable("CommonZones");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ApartmentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<int?>("ResidentialUnitId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApartmentId");

                    b.HasIndex("ResidentialUnitId");

                    b.ToTable("Events");

                    b.HasDiscriminator<string>("EventType").HasValue("Event");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.News", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ResidentialUnitId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ResidentialUnitId", "Title")
                        .IsUnique();

                    b.ToTable("News");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.Pet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Breed")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ApartmentId", "Name")
                        .IsUnique();

                    b.ToTable("Pets");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.Pqrs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(3000)
                        .HasColumnType("nvarchar(3000)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("PqrsState")
                        .HasColumnType("int");

                    b.Property<int>("PqrsType")
                        .HasColumnType("int");

                    b.Property<int>("ResidentialUnitId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApartmentId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("ResidentialUnitId");

                    b.ToTable("Pqrss");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.PqrsMovement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Observation")
                        .IsRequired()
                        .HasMaxLength(3000)
                        .HasColumnType("nvarchar(3000)");

                    b.Property<int>("PqrsId")
                        .HasColumnType("int");

                    b.Property<int>("PqrsState")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PqrsId");

                    b.ToTable("PqrsMovements");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.ResidentialUnit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("Name", "CityId")
                        .IsUnique();

                    b.ToTable("ResidentialUnits");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId", "Name")
                        .IsUnique();

                    b.ToTable("States");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("ApartmentId")
                        .HasColumnType("int");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Document")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ResidentialUnitId")
                        .HasColumnType("int");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApartmentId");

                    b.HasIndex("CityId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("ResidentialUnitId");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Plate")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ApartmentId", "Plate")
                        .IsUnique();

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.CommonZoneReservation", b =>
                {
                    b.HasBaseType("CommUnity.Shared.Entities.Event");

                    b.Property<int>("CommonZoneId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasIndex("CommonZoneId");

                    b.HasDiscriminator().HasValue("CommonZoneReservation");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.MailArrival", b =>
                {
                    b.HasBaseType("CommUnity.Shared.Entities.Event");

                    b.Property<string>("Sender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Events", t =>
                        {
                            t.Property("Status")
                                .HasColumnName("MailArrival_Status");
                        });

                    b.HasDiscriminator().HasValue("MailArrival");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.VisitorEntry", b =>
                {
                    b.HasBaseType("CommUnity.Shared.Entities.Event");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Plate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.ToTable("Events", t =>
                        {
                            t.Property("Status")
                                .HasColumnName("VisitorEntry_Status");
                        });

                    b.HasDiscriminator().HasValue("VisitorEntry");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.Apartment", b =>
                {
                    b.HasOne("CommUnity.Shared.Entities.ResidentialUnit", "ResidentialUnit")
                        .WithMany("Apartments")
                        .HasForeignKey("ResidentialUnitId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ResidentialUnit");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.City", b =>
                {
                    b.HasOne("CommUnity.Shared.Entities.State", "State")
                        .WithMany("Cities")
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("State");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.CommonZone", b =>
                {
                    b.HasOne("CommUnity.Shared.Entities.ResidentialUnit", "ResidentialUnit")
                        .WithMany("CommonZones")
                        .HasForeignKey("ResidentialUnitId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ResidentialUnit");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.Event", b =>
                {
                    b.HasOne("CommUnity.Shared.Entities.Apartment", "Apartment")
                        .WithMany("Events")
                        .HasForeignKey("ApartmentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CommUnity.Shared.Entities.ResidentialUnit", "ResidentialUnit")
                        .WithMany("Events")
                        .HasForeignKey("ResidentialUnitId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Apartment");

                    b.Navigation("ResidentialUnit");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.News", b =>
                {
                    b.HasOne("CommUnity.Shared.Entities.ResidentialUnit", "ResidentialUnit")
                        .WithMany("News")
                        .HasForeignKey("ResidentialUnitId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ResidentialUnit");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.Pet", b =>
                {
                    b.HasOne("CommUnity.Shared.Entities.Apartment", "Apartment")
                        .WithMany("Pets")
                        .HasForeignKey("ApartmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Apartment");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.Pqrs", b =>
                {
                    b.HasOne("CommUnity.Shared.Entities.Apartment", "Apartment")
                        .WithMany("Pqrss")
                        .HasForeignKey("ApartmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CommUnity.Shared.Entities.ResidentialUnit", "ResidentialUnit")
                        .WithMany("Pqrss")
                        .HasForeignKey("ResidentialUnitId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Apartment");

                    b.Navigation("ResidentialUnit");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.PqrsMovement", b =>
                {
                    b.HasOne("CommUnity.Shared.Entities.Pqrs", "Pqrs")
                        .WithMany("PqrsMovements")
                        .HasForeignKey("PqrsId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Pqrs");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.ResidentialUnit", b =>
                {
                    b.HasOne("CommUnity.Shared.Entities.City", "City")
                        .WithMany("ResidentialUnits")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.State", b =>
                {
                    b.HasOne("CommUnity.Shared.Entities.Country", "Country")
                        .WithMany("States")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.User", b =>
                {
                    b.HasOne("CommUnity.Shared.Entities.Apartment", "Apartment")
                        .WithMany("Users")
                        .HasForeignKey("ApartmentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CommUnity.Shared.Entities.City", "City")
                        .WithMany("Users")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CommUnity.Shared.Entities.ResidentialUnit", "ResidentialUnit")
                        .WithMany("Users")
                        .HasForeignKey("ResidentialUnitId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Apartment");

                    b.Navigation("City");

                    b.Navigation("ResidentialUnit");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.Vehicle", b =>
                {
                    b.HasOne("CommUnity.Shared.Entities.Apartment", "Apartment")
                        .WithMany("Vehicles")
                        .HasForeignKey("ApartmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Apartment");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CommUnity.Shared.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CommUnity.Shared.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CommUnity.Shared.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CommUnity.Shared.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.CommonZoneReservation", b =>
                {
                    b.HasOne("CommUnity.Shared.Entities.CommonZone", "CommonZone")
                        .WithMany("Reservations")
                        .HasForeignKey("CommonZoneId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CommonZone");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.Apartment", b =>
                {
                    b.Navigation("Events");

                    b.Navigation("Pets");

                    b.Navigation("Pqrss");

                    b.Navigation("Users");

                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.City", b =>
                {
                    b.Navigation("ResidentialUnits");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.CommonZone", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.Country", b =>
                {
                    b.Navigation("States");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.Pqrs", b =>
                {
                    b.Navigation("PqrsMovements");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.ResidentialUnit", b =>
                {
                    b.Navigation("Apartments");

                    b.Navigation("CommonZones");

                    b.Navigation("Events");

                    b.Navigation("News");

                    b.Navigation("Pqrss");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("CommUnity.Shared.Entities.State", b =>
                {
                    b.Navigation("Cities");
                });
#pragma warning restore 612, 618
        }
    }
}
