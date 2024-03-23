using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Queries.GetAll;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class RecipeTagConverter : IToDomainConverter<RecipeTagContract, RecipeTag>
{
    public RecipeTag ToDomain(RecipeTagContract source)
    {
        return new RecipeTag(source.Id, source.Name);
    }
}