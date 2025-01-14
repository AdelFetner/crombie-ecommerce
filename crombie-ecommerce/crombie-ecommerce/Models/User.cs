﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace crombie_ecommerce.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [MinLength(4)]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        public bool IsVerified { get; set; }

        [JsonIgnore]
        public Guid? ProductId { get; set; }
        [JsonIgnore]
        public  Product? Product { get; set; }
       
        public virtual Wishlist? Wishlist { get; set; }

    }
}
