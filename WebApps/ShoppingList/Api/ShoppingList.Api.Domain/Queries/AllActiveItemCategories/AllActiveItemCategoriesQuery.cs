using ShoppingList.Api.Domain.Queries.SharedModels;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.Queries.AllActiveItemCategories
{
    public class AllActiveItemCategoriesQuery : IQuery<IEnumerable<ItemCategoryReadModel>>
    {
    }
}