using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.RecipeTags.Queries.GetAll;

public class GetAllQuery : IQuery<IEnumerable<IRecipeTag>>
{
}