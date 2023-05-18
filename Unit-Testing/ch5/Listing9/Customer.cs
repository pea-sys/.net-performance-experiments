
namespace ch5.Listing9
{
    internal class Customer
    {
        public bool Purchase(IStore store, Product product, int quantity)
        {
            if (!store.HasEnoughInventory(product, quantity))
            {
                return false;
            }

            store.RemoveInventory(product, quantity);

            return true;
        }

        public string Email { get; set; }
    }
}
