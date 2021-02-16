using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Models
{
    public static class StoreSectionExtensions
    {
        public static Section ToEntity(this IStoreSection model)
        {
            return new Section
            {
                Id = model.Id.Value,
                Name = model.Name,
                SortIndex = model.SortingIndex
            };
        }
    }
}