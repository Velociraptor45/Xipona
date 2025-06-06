using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Recipes.Models;
using Xipona.Api.Domain.Recipes.Services.Queries;

namespace Xipona.Api.ApplicationServices.Recipes.Queries.RecipeById;

public class RecipeByIdQuery : IQuery<RecipeReadModel>
{
    public RecipeByIdQuery(RecipeId recipeId)
    {
        RecipeId = recipeId;
    }

    public RecipeId RecipeId { get; }
}