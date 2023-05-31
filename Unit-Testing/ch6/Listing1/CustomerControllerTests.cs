

namespace ch6.Listing1
{
    /// <summary>
    /// 出力値ベーステストサンプル
    /// </summary>
    public class CustomerControllerTests
    {
        [Fact]
        public void Discount_of_two_products()
        {
            var product1 = new Product("Hand wash");
            var product2 = new Product("Shampoo");
            var sut = new PriceEngine();

            decimal discount = sut.CalculateDiscount(
                product1, product2);

            Assert.Equal(0.02m, discount);
        }
    }
}
