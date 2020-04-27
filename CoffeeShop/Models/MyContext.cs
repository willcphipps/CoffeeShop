using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Models {
    public class MyContext : DbContext {
        public MyContext (DbContextOptions options) : base (options) { }
        DbSet<Category> Categories { get; set; }
        DbSet<Checkout> Checouts { get; set; }
        DbSet<Collection> Collections { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<Product> Products { get; set; }
    }
}