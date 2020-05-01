using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CoffeeShop.Models {
    public class CreateProduct {
        // public int ProductId { get; set; }
        public string ProductName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }
        public int CategoryId { get; set; }
        public List<Collection> inCategory { get; set; }
        public List<OrderItem> ListofItems { get; set; }
        public IFormFile ImagePath { get; set; }

    }
}