using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Recipes.Models;
using Xipona.Api.Domain.Recipes.Services.Queries;
using Recipe = Xipona.Api.Repositories.Recipes.Entities.Recipe;

namespace Xipona.Api.Repositories.Recipes.Converters.ToDomain;

public class RecipeSearchResultConverter : IToDomainConverter<Entities.Recipe, RecipeSearchResult>
{
    public RecipeSearchResult ToDomain(Recipe source)
    {
        return new RecipeSearchResult(new RecipeId(source.Id), new RecipeName(source.Name));
    }
}