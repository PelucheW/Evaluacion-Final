using Microsoft.EntityFrameworkCore;
using ProyectoFinalTecWeb.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace ProyectoFinalTecWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Trip> Trips => Set<Trip>();
        public DbSet<Driver> Driveres => Set<Driver>();
        public DbSet<Passenger> Passengers => Set<Passenger>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<Model> Models => Set<Model>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //EN PRUBA

            // 1:N Driver -> Trips
            modelBuilder.Entity<Driver>(d =>
            {
                d.HasMany(d => d.Trips)
                .WithOne(trip => trip.Driver)
                .HasForeignKey(trip => trip.DriverId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            // 1:N Passenger -> Trips
            modelBuilder.Entity<Passenger>(p =>
            {
                p.HasMany(p => p.Trips)
                .WithOne(trip => trip.Passenger)
                .HasForeignKey(trip => trip.PassengerId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            // Relación Driver - Vehículo (1:N)
            modelBuilder.Entity<Driver>()
                .HasMany(c => c.Vehicles)
                .WithOne(v => v.Driver)
                .HasForeignKey(v => v.DriverId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            // Relación  Vehículo - Model (1:1)
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Model)
                .WithOne(m => m.Vehicle)
                .HasForeignKey<Vehicle>(v => v.ModelId)
                .IsRequired() // vehículo debe tener SI O SI un modelo
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}