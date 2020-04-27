using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models {
    public class Category {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<Collection> hasProducts { get; set; }
    }
}