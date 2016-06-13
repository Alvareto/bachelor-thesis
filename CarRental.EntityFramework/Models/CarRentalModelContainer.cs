namespace CarRental.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CarRentalWebSiteContext : DbContext
    {
        public CarRentalWebSiteContext()
            : base("name=CarRentalWebSiteContext")
        {
        }

        public virtual DbSet<CarDetail> CarDetailSet { get; set; }
        public virtual DbSet<Car> CarSet { get; set; }
        public virtual DbSet<Office> OfficeSet { get; set; }
        public virtual DbSet<Reservation> ReservationSet { get; set; }
        public virtual DbSet<Review> ReviewSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
                .HasMany(e => e.CarDetails)
                .WithRequired(e => e.Car)
                .HasForeignKey(e => e.Car_Id);

            modelBuilder.Entity<Car>()
                .HasMany(e => e.Reservations)
                .WithRequired(e => e.Car)
                .HasForeignKey(e => e.Car_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Car>()
                .HasMany(e => e.Reviews)
                .WithRequired(e => e.Car)
                .HasForeignKey(e => e.Car_Id);

            modelBuilder.Entity<Office>()
                .HasMany(e => e.Cars)
                .WithRequired(e => e.Office)
                .HasForeignKey(e => e.Office_Id);

            modelBuilder.Entity<Reservation>()
                .HasMany(e => e.ReviewSet)
                .WithOptional(e => e.Reservation)
                .HasForeignKey(e => e.Reservation_Id);

            modelBuilder.Entity<Reservation>()
                .Property(e => e.Client_Id)
                .IsOptional();
        }
    }
}
