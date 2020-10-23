using ShoppingList.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Domain.Commands.CreateItem
{
    public class CreateItemCommand : ICommand<bool>
    {
        private readonly IEnumerable<StoreItem> storeItems;

        public CreateItemCommand(IEnumerable<StoreItem> storeItems)
        {
            this.storeItems = storeItems;
        }

        public IReadOnlyCollection<StoreItem> StoreItems => storeItems.ToList().AsReadOnly();
    }
}