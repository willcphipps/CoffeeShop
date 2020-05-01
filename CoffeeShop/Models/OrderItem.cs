using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models {
    public class OrderItem

    {
        [Key]
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public Order Orders { get; set; }
        public int ProductId { get; set; }
        public Product ProdinOrder { get; set; }
    }
}