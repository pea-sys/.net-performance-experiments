
namespace ch2.Listing1
{
    public class Customer
    {
        public bool Purchase(Store store, Product product, int quantity)
        {
            if (!store.HasEnoughInventory(product, quantity))
            {
                return false;
            }

            store.RemoveInventory(product, quantity);

            return true;
        }
    }
}
