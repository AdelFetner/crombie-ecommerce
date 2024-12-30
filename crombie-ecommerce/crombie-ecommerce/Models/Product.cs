﻿using System.ComponentModel.DataAnnotations;

namespace crombie_ecommerce.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MinLength(4)]
        [StringLength(50)]
        public string Name { get; set; }

        
        [StringLength(100)]
        public string Description { get; set; }

        
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [StringLength(50)]
        public string Brand { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        //public Guid UserId { get; set; }
        //public virtual User User { get; set; }

        //public Guid WishlistId { get; set; }
        //public virtual Wishlist Wishlist { get; set; }
    }
}