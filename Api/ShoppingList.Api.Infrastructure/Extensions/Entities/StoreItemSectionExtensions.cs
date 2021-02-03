using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Entities
{
    public static class StoreItemSectionExtensions
    {
        public static IStoreItemSection ToStoreItemSectionDomain(this Section entity)
        {
            return new StoreItemSection(new StoreItemSectionId(entity.Id), entity.Name, entity.SortIndex);
        }
    }
}