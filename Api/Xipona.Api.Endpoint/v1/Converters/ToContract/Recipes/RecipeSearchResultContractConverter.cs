using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Recipes;

public class RecipeSearchResultContractConverter : IToContractConverter<RecipeSearchResult, RecipeSearchResultContract>
{
    public RecipeSearchResultContract ToContract(RecipeSearchResult source)
    {
        return new RecipeSearchResultContract(source.Id, source.Name);
    }
}