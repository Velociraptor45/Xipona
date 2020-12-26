using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public interface IStoreItemFactory
    {
        IStoreItem Create(ItemCreation itemCreation, IItemCategory itemCategory, IManufacturer manufacturer);

        IStoreItem Create(TemporaryItemCreation model);
    }
}