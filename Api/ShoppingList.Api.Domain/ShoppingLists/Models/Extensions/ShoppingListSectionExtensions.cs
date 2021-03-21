using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Extensions
{
    public static class ShoppingListSectionExtensions
    {
        public static ShoppingListSectionReadModel ToReadModel(this IShoppingListSection model)
        {
            return new ShoppingListSectionReadModel(model.Id, model.Name, model.SortingIndex, model.IsDefaultSection,
                model.ShoppingListItems.Select(i => i.ToReadModel()));
        }
    }
}
