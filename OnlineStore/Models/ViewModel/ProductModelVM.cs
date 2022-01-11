using System.Collections.Generic;

namespace OnlineStore.Models.ViewModel
{
    public class ProductModelVM
    {
        public ProductModelVM()
        {
            ProductList = new List<Product>();
        }

        public User User { get; set; }
        public IList<Product> ProductList { get; set; }
    }
}