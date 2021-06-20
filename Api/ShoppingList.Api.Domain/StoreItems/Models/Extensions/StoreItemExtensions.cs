using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemFilterResults;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions
{
    public static class StoreItemExtensions
    {
        public static ItemFilterResultReadModel ToItemFilterResultReadModel(this IStoreItem model)
        {
            return new ItemFilterResultReadModel(model.Id, model.Name);
        }
    }
}