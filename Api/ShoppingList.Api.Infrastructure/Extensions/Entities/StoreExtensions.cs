using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Entities
{
    public static class StoreExtensions
    {
        public static IStore ToDomain(this Infrastructure.Entities.Store entity)
        {
            return new Store(
                new Domain.Stores.Model.StoreId(entity.Id),
                entity.Name,
                entity.Deleted,
                entity.Sections
                    .Select(section => new StoreSection(
                        new StoreSectionId(section.Id),
                        section.Name,
                        section.SortIndex,
                        section.Id == entity.DefaultSectionId)));
        }

        public static IShoppingListStore ToShoppingListDomain(this Infrastructure.Entities.Store entity)
        {
            return new ShoppingListStore(
                new Domain.ShoppingLists.Models.ShoppingListStoreId(entity.Id),
                entity.Name,
                entity.Deleted);
        }
    }
}