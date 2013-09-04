using ShoppingCartService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartService.Repository.Interface
{
    public interface IShoppingCartRepository
    {
        void AddItem(Item item);

        void UpdateItem(Item item);

        void RemoveItem();

        List<Item> GetItems();
    }
}
