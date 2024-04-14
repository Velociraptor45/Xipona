using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Queries.RecipeById;

public class RecipeByIdQuery : IQuery<RecipeReadModel>
{
    public RecipeByIdQuery(RecipeId recipeId)
    {
        RecipeId = recipeId;
    }

    public RecipeId RecipeId { get; }
}