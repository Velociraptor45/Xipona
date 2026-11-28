using Xipona.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Recipes.States;

namespace Xipona.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class RecipeSearchResultConverter : IToDomainConverter<RecipeSearchResultContract, RecipeSearchResult>
{
    public RecipeSearchResult ToDomain(RecipeSearchResultContract source)
    {
        return new RecipeSearchResult(source.Id, source.Name);
    }
}