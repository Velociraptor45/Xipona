using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class RecipeSearchResultConverter : IToDomainConverter<RecipeSearchResultContract, RecipeSearchResult>
{
    public RecipeSearchResult ToDomain(RecipeSearchResultContract source)
    {
        return new RecipeSearchResult(source.Id, source.Name);
    }
}