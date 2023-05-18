using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ch5.Listing9
{
    public interface IStore
    {
        bool HasEnoughInventory(Product product, int quantity);
        void RemoveInventory(Product product, int quantity);
        void AddInventory(Product product, int quantity);
        int GetInventory(Product product);
    }
}
