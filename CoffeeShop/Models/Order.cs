using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models {
    public class Order {
        [Key]
        public int OrderId { get; set; }
        public int TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public Customer OrderFor { get; set; }
        public DateTime OrderDate { get; set; }
        public List<Checkout> Cart { get;set; }

    }
}