using Xipona.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Recipes.Services.Queries;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.Recipes;

public class RecipeSearchResultContractConverter : IToContractConverter<RecipeSearchResult, RecipeSearchResultContract>
{
    public RecipeSearchResultContract ToContract(RecipeSearchResult source)
    {
        return new RecipeSearchResultContract(source.Id, source.Name);
    }
}