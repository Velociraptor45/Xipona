using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries.Quantities;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Queries.AllIngredientQuantityTypes;

public class AllIngredientQuantityTypesQuery : IQuery<IEnumerable<IngredientQuantityTypeReadModel>>
{
}