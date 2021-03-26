using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions
{
    public static class StoreSectionExtensions
    {
        public static StoreSectionReadModel ToReadModel(this IStoreSection model)
        {
            return new StoreSectionReadModel(model.Id, model.Name, model.SortingIndex, model.IsDefaultSection);
        }

        public static StoreItemSectionReadModel ToItemSectionReadModel(this IStoreSection model)
        {
            return new StoreItemSectionReadModel(model.Id, model.Name, model.SortingIndex);
        }
    }
}