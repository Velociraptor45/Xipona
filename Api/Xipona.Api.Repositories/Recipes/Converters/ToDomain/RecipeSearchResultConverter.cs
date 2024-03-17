using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using Recipe = ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Recipe;

namespace ProjectHermes.Xipona.Api.Repositories.Recipes.Converters.ToDomain;

public class RecipeSearchResultConverter : IToDomainConverter<Entities.Recipe, RecipeSearchResult>
{
    public RecipeSearchResult ToDomain(Recipe source)
    {
        return new RecipeSearchResult(new RecipeId(source.Id), new RecipeName(source.Name));
    }
}