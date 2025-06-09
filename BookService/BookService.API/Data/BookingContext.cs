using Microsoft.EntityFrameworkCore;
using BookService.Models;
using System;

namespace BookService.Data
{
    public class BookingContext : DbContext
    {
        public virtual DbSet<Booking> Bookings { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is null.</exception>
        public BookingContext(DbContextOptions<BookingContext> options)
            : base(options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguration der Booking-Entität
            modelBuilder.Entity<Booking>(entity =>
            {
                // Primärschlüssel definieren
                entity.HasKey(e => e.Id);

                // Tabellenname (optional, da EF Core den Klassennamen verwendet)
                entity.ToTable("Bookings");

                // Eigenschaften konfigurieren (z. B. Pflichtfelder)
                entity.Property(e => e.FlightId).IsRequired();
                entity.Property(e => e.PassengerId).IsRequired();
                entity.Property(e => e.PassengerFirstname).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PassengerLastname).IsRequired().HasMaxLength(100);
                entity.Property(e => e.TicketCount).IsRequired();
            });
        }
    }
}