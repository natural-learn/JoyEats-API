using JoyEats.Models;
using Microsoft.EntityFrameworkCore;

namespace JoyEats.EntityFrameworkCore
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<DishFlavor> DishFlavors { get; set; }

        public DbSet<AddressBook> AddressBook { get; set; }

        public DbSet<OrderDetail> OrderDetail { get; set; }

        public DbSet<Orders> Orders { get; set; }

        public DbSet<Setmeal> Setmeal { get; set; }

        public DbSet<SetmealDish> SetmealDishes { get; set; }

        public DbSet<ShoppingCart> shoppingCarts { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
