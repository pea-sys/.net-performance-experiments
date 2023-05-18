using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ch5.Listing9
{
    public class CustomerController
    {
        private readonly CustomerRepository _customerRepository;
        private readonly ProductRepository _productRepository;
        private readonly Store _mainStore;
        private readonly IEmailGateway _emailGateway;

        public CustomerController(IEmailGateway emailGateway)
        {
            _emailGateway = emailGateway;
        }

        public bool Purchase(int customerId, int productId, int quantity)
        {
            Customer customer = _customerRepository.GetById(customerId);
            Product product = _productRepository.GetById(productId);

            bool isSuccess = customer.Purchase(_mainStore, product, quantity);

            if (isSuccess)
            {
                _emailGateway.SendReceipt(customer.Email, product.Name, quantity);
            }

            return isSuccess;
        }
    }
}
