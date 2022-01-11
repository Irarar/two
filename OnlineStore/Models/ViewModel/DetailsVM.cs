namespace OnlineStore.Models.ViewModel
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            Product = new Product();
        }

        public Product Product { get; set; }
        public bool Basket { get; set; }
    }
}