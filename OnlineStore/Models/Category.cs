using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace OnlineStore.Models
{
    public class Category
    {
        public Guid Id { get; set; }

        [Required]
        [DisplayName("Name Category")]
        public string Name { get; set; }

        public Store Store { get; set; }
        [Required]
        [DisplayName("Online Store")]
        public Guid StoreId { get; set; }
        public List<Product> Product { get; set; }

    }
}