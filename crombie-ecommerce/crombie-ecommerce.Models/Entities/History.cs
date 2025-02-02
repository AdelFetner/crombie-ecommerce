using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crombie_ecommerce.Models.Entities
{
    public abstract class History
    {
        [Key]
        public Guid OriginalId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public DateTime ProcessedAt { get; set; }
        [Required]
        public string ProcessedBy { get; set; }
        [Required]
        public string EntityJson { get; set; }
    }
}
