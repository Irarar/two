using System.Collections.Generic;

namespace OnlineStore.Models.ViewModel
{
    public class HomeVM
    {
        public List<Product> Product { get; set; } = new List<Product>();
        public List<Category> Category { get; set; } = new List<Category>();
    }
}