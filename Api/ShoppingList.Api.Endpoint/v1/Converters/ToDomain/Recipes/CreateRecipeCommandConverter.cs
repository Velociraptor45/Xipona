using ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Commands.CreateRecipe;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.CreateRecipe;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Recipes;

public class CreateRecipeCommandConverter : IToDomainConverter<CreateRecipeContract, CreateRecipeCommand>
{
    public CreateRecipeCommand ToDomain(CreateRecipeContract source)
    {
        var steps = source.PreparationSteps.Select(s => new PreparationStepCreation(
            new PreparationStepInstruction(s.Instruction),
            s.SortingIndex));

        var ingredients = source.Ingredients.Select(i => new IngredientCreation(
            new ItemCategoryId(i.ItemCategoryId),
            i.QuantityType.ToEnum<IngredientQuantityType>(),
            new IngredientQuantity(i.Quantity)));

        var creation = new RecipeCreation(
            new RecipeName(source.Name),
            ingredients,
            steps);

        return new CreateRecipeCommand(creation);
    }
}