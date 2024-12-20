﻿using CommUnity.Shared.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CommUnity.BackEnd.Data
{
    //public class DataContext : DbContext
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.SetCommandTimeout(600);
        }

        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CommonZone> CommonZones { get; set; }
        public DbSet<CommonZoneReservation> CommonZoneReservations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<MailArrival> MailArrivals { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<ResidentialUnit> ResidentialUnits { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VisitorEntry> VisitorEntries { get; set; }
        public DbSet<Pqrs> Pqrss { get; set; }
        public DbSet<PqrsMovement> PqrsMovements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(
                    @"name=LocalConnection",
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Apartment>().HasIndex(x => new { x.ResidentialUnitId, x.Number }).IsUnique();
            modelBuilder.Entity<City>().HasIndex(x => new { x.StateId, x.Name }).IsUnique();
            modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<CommonZone>().HasIndex(x => new { x.ResidentialUnitId, x.Name }).IsUnique();
            modelBuilder.Entity<Event>().HasDiscriminator<string>("EventType").HasValue<MailArrival>("MailArrival").HasValue<CommonZoneReservation>("CommonZoneReservation").HasValue<VisitorEntry>("VisitorEntry");    
            modelBuilder.Entity<News>().HasIndex(x => new { x.ResidentialUnitId, x.Title }).IsUnique();
            modelBuilder.Entity<Pet>().HasIndex(x => new { x.ApartmentId, x.Name }).IsUnique();
            modelBuilder.Entity<ResidentialUnit>().HasIndex(x => new { x.Name, x.CityId }).IsUnique();
            modelBuilder.Entity<State>().HasIndex(x => new { x.CountryId, x.Name }).IsUnique();
            modelBuilder.Entity<Vehicle>().HasIndex(x => new { x.ApartmentId, x.Plate }).IsUnique();
            modelBuilder.Entity<Pqrs>().HasIndex(x => new { x.Id }).IsUnique();
            modelBuilder.Entity<PqrsMovement>().HasIndex(x => new { x.Id }).IsUnique();

            DisableCascadingDelete(modelBuilder);
        }

        private void DisableCascadingDelete(ModelBuilder modelBuilder)
        {
            var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
            foreach (var relationship in relationships)
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}