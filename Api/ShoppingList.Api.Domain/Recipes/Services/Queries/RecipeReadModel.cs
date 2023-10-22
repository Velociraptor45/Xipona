using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
public record RecipeReadModel(RecipeId Id, RecipeName Name, NumberOfServings NumberOfServings,
    IReadOnlyCollection<IngredientReadModel> Ingredients,
    IReadOnlyCollection<PreparationStepReadModel> PreparationSteps, IReadOnlyCollection<RecipeTagId> Tags);