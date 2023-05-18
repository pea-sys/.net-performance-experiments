using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ch5.Listing9
{
    public class CustomerControllerTests
    {
        [Fact(Skip = "Concept illustration only")]
        public void Successful_purchase()
        {
            var mock = new Mock<IEmailGateway>();
            var sut = new CustomerController(mock.Object);

            bool isSuccess = sut.Purchase(
                customerId: 1, productId: 2, quantity: 5);

            Assert.True(isSuccess);
            mock.Verify(
                x => x.SendReceipt(
                    "customer@email.com", "Shampoo", 5),
                Times.Once);
        }
    }
}
