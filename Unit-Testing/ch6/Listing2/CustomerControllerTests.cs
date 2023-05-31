
namespace ch6.Listing2
{
    /// <summary>
    /// 状態ベーステストサンプル
    /// </summary>
    public class CustomerControllerTests
    {
        [Fact]
        public void Adding_a_product_to_an_order()
        {
            var product = new Product("Hand wash");
            var sut = new Order();

            sut.AddProduct(product);

            Assert.Equal(1, sut.Products.Count);
            Assert.Equal(product, sut.Products[0]);
        }
    }
}
