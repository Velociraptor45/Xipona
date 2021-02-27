using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Entities
{
    public static class StoreExtensions
    {
        public static IShoppingListStore ToShoppingListDomain(this Infrastructure.Entities.Store entity)
        {
            return new ShoppingListStore(
                new Domain.ShoppingLists.Models.ShoppingListStoreId(entity.Id),
                entity.Name,
                entity.Deleted);
        }

        public static IStoreItemStore ToStoreItemDomain(this Infrastructure.Entities.Store entity)
        {
            return new StoreItemStore(
                new StoreItemStoreId(entity.Id),
                entity.Name,
                entity.Sections.Select(s => s.ToStoreItemSectionDomain()));
        }
    }
}