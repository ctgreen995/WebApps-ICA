using Microsoft.EntityFrameworkCore;

namespace ThAmCo.Catering.Data
{
    public class CateringDbContext : DbContext
    {
        public DbSet<FoodBooking> FoodBookings { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuFoodItem> MenuFoodItems { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }

        private readonly string DbPath;

        public CateringDbContext(DbContextOptions options) : base(options)
        {
            var folder = @"./Data";
            DbPath = Path.Join(folder, "ThAmCo.Catering.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FoodBooking>()
                .HasOne(m => m.Menu)
                .WithMany(f => f.FoodBookings);

            builder.Entity<MenuFoodItem>()
                .HasKey(k => new { k.MenuId, k.FoodItemId });

            builder.Entity<Menu>()
                .HasMany(fi => fi.MenuFoodItems)
                .WithOne(m => m.Menu)
                .HasForeignKey(m => m.MenuId);

            builder.Entity<FoodItem>()
                .HasMany(m => m.Menus)
                .WithOne(fi => fi.FoodItem)
                .HasForeignKey(fi => fi.FoodItemId);

            builder.Entity<MenuFoodItem>()
                .HasData(new MenuFoodItem { MenuId = 1, FoodItemId = 5 },
                new MenuFoodItem { MenuId = 1, FoodItemId = 6 },
                new MenuFoodItem { MenuId = 2, FoodItemId = 3 },
                new MenuFoodItem { MenuId = 2, FoodItemId = 4 },
                new MenuFoodItem { MenuId = 3, FoodItemId = 1 },
                new MenuFoodItem { MenuId = 3, FoodItemId = 2 }
                );

            builder.Entity<FoodBooking>()
                .HasData(
                new FoodBooking { ClientReferenceId = "Green", FoodBookingId = 123, MenuId = 1, NumberOfGuests = 10 }
                );

            builder.Entity<FoodItem>()
                .HasData(
                new FoodItem { Id = 1, Description = "Enchiladas and Chips", UnitPrice = 4.00 },
                new FoodItem { Id = 2, Description = "Tacos and Chips", UnitPrice = 6.00 },
                new FoodItem { Id = 3, Description = "Egg and Chips", UnitPrice = 5.00 },
                new FoodItem { Id = 4, Description = "Sunday Dinner and Chips", UnitPrice = 10.00 },
                new FoodItem { Id = 5, Description = "Broccoli and Chips", UnitPrice = 8.00 },
                new FoodItem { Id = 6, Description = "Avacado and Chips", UnitPrice = 7.00 }
                );

            builder.Entity<Menu>()
                .HasData(
                new Menu { Id = 1, Name = "Veggie" },
                new Menu { Id = 2, Name = "English" },
                new Menu { Id = 3, Name = "Mexican" });
        }
    }
}
