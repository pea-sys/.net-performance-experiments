
namespace ch6.Listing2
{
    public class Order
    {
        private readonly List<Product> _products = new List<Product>();
        public IReadOnlyList<Product> Products => _products.ToList();

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }
    }
}
