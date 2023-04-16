using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Queries.SearchByTagIds;

public class SearchRecipesByTagsQuery : IQuery<IEnumerable<RecipeSearchResult>>
{
    public SearchRecipesByTagsQuery(IEnumerable<RecipeTagId> tagIds)
    {
        TagIds = tagIds.ToList();
    }

    public IReadOnlyCollection<RecipeTagId> TagIds { get; }
}