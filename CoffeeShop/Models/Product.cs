using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CoffeeShop.Models {
    public class Product {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }
        // public int CategoryId { get; set; }
        public List<Collection> inCategory { get; set; }
        // public Order ProductsinOrder { get; set; }
        // public int OrderId { get; set; }
        public List<OrderItem> ListofItems { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}