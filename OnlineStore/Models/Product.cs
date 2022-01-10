using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, Double.MaxValue)]
        public double Price { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        [DisplayName ("Category Type")]
        public Guid CategoryId { get; set; }

        public Category Category { get; set; }
    }
}