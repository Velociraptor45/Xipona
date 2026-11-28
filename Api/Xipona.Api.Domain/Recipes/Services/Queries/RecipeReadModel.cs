using Xipona.Api.Domain.Recipes.Models;
using Xipona.Api.Domain.RecipeTags.Models;

namespace Xipona.Api.Domain.Recipes.Services.Queries;
public record RecipeReadModel(RecipeId Id, RecipeName Name, NumberOfServings NumberOfServings,
    IReadOnlyCollection<IngredientReadModel> Ingredients,
    IReadOnlyCollection<PreparationStepReadModel> PreparationSteps, IReadOnlyCollection<RecipeTagId> Tags,
    SideDishReadModel? SideDish);