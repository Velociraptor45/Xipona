using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Queries.ItemAmountsForOneServing;

public class ItemAmountsForOneServingQuery : IQuery<IEnumerable<ItemAmountForOneServing>>
{
    public ItemAmountsForOneServingQuery(RecipeId recipeId)
    {
        RecipeId = recipeId;
    }

    public RecipeId RecipeId { get; }
}