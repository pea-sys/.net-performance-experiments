
namespace ch3.Listing13
{
    public class DeliveryService
    {
        public bool IsDeliveryValid(Delivery delivery)
        {
            return delivery.Date >= DateTime.Now.AddDays(1.999);
        }
    }
}
