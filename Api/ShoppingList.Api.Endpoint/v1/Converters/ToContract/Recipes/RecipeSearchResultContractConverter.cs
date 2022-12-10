using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Recipes;

public class RecipeSearchResultContractConverter : IToContractConverter<RecipeSearchResult, RecipeSearchResultContract>
{
    public RecipeSearchResultContract ToContract(RecipeSearchResult source)
    {
        return new RecipeSearchResultContract(source.Id.Value, source.Name);
    }
}