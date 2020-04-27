using System;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models {
    public class Collection {
        [Key]
        public int CollectionId { get; set; }
        public int ProductId { get; set; }
        public Product NavProd { get; set; }
        public Category NavCat { get; set; }
    }
}