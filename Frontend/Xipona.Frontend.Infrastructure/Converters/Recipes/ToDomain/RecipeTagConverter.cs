using Xipona.Api.Contracts.RecipeTags.Queries.GetAll;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Recipes.States;

namespace Xipona.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class RecipeTagConverter : IToDomainConverter<RecipeTagContract, RecipeTag>
{
    public RecipeTag ToDomain(RecipeTagContract source)
    {
        return new RecipeTag(source.Id, source.Name);
    }
}