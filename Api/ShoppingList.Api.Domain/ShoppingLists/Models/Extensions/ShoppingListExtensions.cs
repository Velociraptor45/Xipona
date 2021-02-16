using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.SharedModels;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Extensions
{
    public static class ShoppingListExtensions
    {
        public static ShoppingListReadModel ToReadModel(this IShoppingList model)
        {
            return new ShoppingListReadModel(model.Id, model.CompletionDate, model.Store.ToCommonStoreReadModel(),
                model.Sections.Select(item => item.ToReadModel()));
        }
    }
}