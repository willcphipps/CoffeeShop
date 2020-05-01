using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models {
    public class Order {
        [Key]
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public int UserId { get; set; }
        public bool HasPaid { get; set; } = false;
        public Customer OrderFor { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public List<OrderItem> prodInOrder { get; set; }
    }
}