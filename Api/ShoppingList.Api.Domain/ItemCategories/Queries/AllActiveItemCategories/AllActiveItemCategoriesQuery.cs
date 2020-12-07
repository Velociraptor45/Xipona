using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.AllActiveItemCategories
{
    public class AllActiveItemCategoriesQuery : IQuery<IEnumerable<ItemCategoryReadModel>>
    {
    }
}