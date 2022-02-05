using System.Collections.Generic;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.AllActiveItemCategories;

public class AllActiveItemCategoriesQuery : IQuery<IEnumerable<ItemCategoryReadModel>>
{
}