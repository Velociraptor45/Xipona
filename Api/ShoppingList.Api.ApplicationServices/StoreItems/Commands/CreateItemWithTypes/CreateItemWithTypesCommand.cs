using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.CreateItemWithTypes
{
    public class CreateItemWithTypesCommand : ICommand<bool>
    {
        public CreateItemWithTypesCommand(IStoreItem item)
        {
            Item = item;
        }

        public IStoreItem Item { get; }
    }
}