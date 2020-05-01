using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Models {
    public class MyContext : DbContext {
        public MyContext (DbContextOptions options) : base (options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}