using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Queries.SearchByTagIds;

public class SearchRecipesByTagsQuery : IQuery<IEnumerable<RecipeSearchResult>>
{
    public SearchRecipesByTagsQuery(IEnumerable<RecipeTagId> tagIds)
    {
        TagIds = tagIds.ToList();
    }

    public IReadOnlyCollection<RecipeTagId> TagIds { get; }
}