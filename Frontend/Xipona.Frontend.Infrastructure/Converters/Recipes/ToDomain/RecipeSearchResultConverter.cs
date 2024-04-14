using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class RecipeSearchResultConverter : IToDomainConverter<RecipeSearchResultContract, RecipeSearchResult>
{
    public RecipeSearchResult ToDomain(RecipeSearchResultContract source)
    {
        return new RecipeSearchResult(source.Id, source.Name);
    }
}