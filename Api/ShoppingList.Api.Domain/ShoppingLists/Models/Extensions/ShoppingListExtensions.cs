using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.SharedModels;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Extensions
{
    public static class ShoppingListExtensions
    {
        public static ShoppingListReadModel ToReadModel(this ShoppingList model)
        {
            return new ShoppingListReadModel(model.Id, model.CompletionDate, model.Store.ToStoreReadModel(),
                model.Items.Select(item => item.ToReadModel()));
        }
    }
}