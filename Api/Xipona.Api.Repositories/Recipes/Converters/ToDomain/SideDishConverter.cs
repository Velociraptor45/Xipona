using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Recipes.Models;
using Xipona.Api.Domain.Recipes.Services.Queries;
using Recipe = Xipona.Api.Repositories.Recipes.Entities.Recipe;

namespace Xipona.Api.Repositories.Recipes.Converters.ToDomain;
public class SideDishConverter : IToDomainConverter<Recipe, SideDishReadModel>
{
    public SideDishReadModel ToDomain(Recipe source)
    {
        return new SideDishReadModel(new RecipeId(source.Id), new RecipeName(source.Name));
    }
}
