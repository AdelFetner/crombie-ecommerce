﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace crombie_ecommerce.Models.Models.Entities
{
    public class Category
    {
        [Key]
        [JsonIgnore]
        public Guid CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
