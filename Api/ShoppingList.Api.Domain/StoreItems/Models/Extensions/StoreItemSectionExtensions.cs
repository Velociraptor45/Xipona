using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions
{
    public static class StoreItemSectionExtensions
    {
        public static StoreItemSectionReadModel ToReadModel(this IStoreItemSection model)
        {
            return new StoreItemSectionReadModel(
                model.Id,
                model.Name,
                model.SortIndex);
        }
    }
}