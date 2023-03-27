using Microsoft.EntityFrameworkCore;

namespace ThAmCo.Events.Events.Data
{
    public class EventsDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<GuestBooking> GuestBookings { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Employee> Staff { get; set; }
        public DbSet<EventStaff> EventStaff { get; set; }

        private readonly string DbPath;

        public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options)
        {
            var folder = @"./Events/Data";
            DbPath = Path.Join(folder, "ThAmCo.Events.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<EventStaff>()
                .HasKey(e => new { e.EmployeeId, e.EventId });

            builder.Entity<Event>()
                .HasMany(s => s.Staff)
                .WithOne(e => e.Event)
                .HasForeignKey(e => e.EventId);

            builder.Entity<Employee>()
                .HasMany(e => e.Events)
                .WithOne(em => em.Employee)
                .HasForeignKey(em => em.EmployeeId);

            builder.Entity<GuestBooking>()
                .HasKey(gb => new { gb.EventBookingId, gb.GuestId });

            builder.Entity<Guest>()
                .HasMany(b => b.Bookings)
                .WithOne(g => g.Guest)
                .HasForeignKey(g => g.GuestId);

            builder.Entity<Event>()
                .HasMany(g => g.Guests)
                .WithOne(e => e.Event)
                .HasForeignKey(e => e.EventBookingId);

            builder.Entity<Employee>()
                .HasData(
                new Employee { Id = 1, Name = "Rishi Sunak" },
                new Employee { Id = 2, Name = "Boris Johnson" },
                new Employee { Id = 3, Name = "Jeremy Hunt" },
                new Employee { Id = 4, Name = "Suella Braverman" },
                new Employee { Id = 5, Name = "Dominic Raab" },
                new Employee { Id = 6, Name = "Michael Levelup Gove" }
                );


            builder.Entity<Guest>()
                .HasData(
                new Guest { Id = 1, Name = "Adam Healey", Street = "11 Kilburn Avenue", Town = "Fairfield", Postcode = "TS196HB", Email = "ahealey@gmail.com", Telephone = "07694836281" },
                new Guest { Id = 2, Name = "Emily Healey", Street = "11 Kilburn Avenue", Town = "Fairfield", Postcode = "TS196HB", Email = "ehealey@gmail.com", Telephone = "07349548361" },
                new Guest { Id = 3, Name = "Heather Nicholson", Street = "6 Owington Grove", Town = "Billingham", Postcode = "TS236LK", Email = "hnicholson@gmail.com", Telephone = "07948567392" },
                new Guest { Id = 4, Name = "Christian Green", Street = "6 Owington Grove", Town = "Billingham", Postcode = "TS236LK", Email = "cgreen@gmail.com", Telephone = "07849573820" },
                new Guest { Id = 5, Name = "James Casey", Street = "3 Southfield Way", Town = "Norton", Postcode = "TS204QD", Email = "jcasey@gmail.com", Telephone = "07968564782" },
                new Guest { Id = 6, Name = "Estelle Jones", Street = "3 Southfield Way", Town = "Norton", Postcode = "TS204QD", Email = "ejones@gmail.com", Telephone = "07946754321" },
                new Guest { Id = 7, Name = "Stuart O'Dell", Street = "5 Brentford Road", Town = "Norton", Postcode = "TS202DW", Email = "sodell@gmail.com", Telephone = "07958940587" },
                new Guest { Id = 8, Name = "Lindsey Wellburn", Street = "5 Brentford Road", Town = "Norton", Postcode = "TS202DW", Email = "lwellburn@gmail.com", Telephone = "07948593058" },
                new Guest { Id = 9, Name = "Stephen Davies", Street = "3 Wellway Walk", Town = "Norton", Postcode = "TS202HW", Email = "sdavies@gmail.com", Telephone = "07960594839" },
                new Guest { Id = 10, Name = "Nicole Davies", Street = "3 Wellway Walk", Town = "Norton", Postcode = "TS202HW", Email = "ndavies@gmail.com", Telephone = "07908967432" }
                );
        }
    }
}
