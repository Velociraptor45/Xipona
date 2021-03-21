using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions
{
    public static class StoreSectionExtensions
    {
        public static StoreSectionReadModel ToReadModel(this IStoreSection model)
        {
            return new StoreSectionReadModel(model.Id, model.Name, model.SortingIndex, model.IsDefaultSection);
        }
    }
}