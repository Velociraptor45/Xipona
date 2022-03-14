using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Searches;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions;

public static class StoreItemExtensions
{
    public static SearchItemResultReadModel ToSearchItemResultReadModel(this IStoreItem model)
    {
        return new SearchItemResultReadModel(model.Id, model.Name);
    }
}