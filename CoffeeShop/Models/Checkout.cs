using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models {
    public class Checkout {
        [Key]
        public int CheckoutId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Product productInOrder { get; set; }
        public Order orderProducts { get; set; }
    }
}