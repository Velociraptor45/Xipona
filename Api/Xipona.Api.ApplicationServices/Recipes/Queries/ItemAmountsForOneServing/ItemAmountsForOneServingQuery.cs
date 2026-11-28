using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Recipes.Models;
using Xipona.Api.Domain.Recipes.Services.Queries;

namespace Xipona.Api.ApplicationServices.Recipes.Queries.ItemAmountsForOneServing;

public class ItemAmountsForOneServingQuery : IQuery<IEnumerable<ItemAmountForOneServing>>
{
    public ItemAmountsForOneServingQuery(RecipeId recipeId)
    {
        RecipeId = recipeId;
    }

    public RecipeId RecipeId { get; }
}