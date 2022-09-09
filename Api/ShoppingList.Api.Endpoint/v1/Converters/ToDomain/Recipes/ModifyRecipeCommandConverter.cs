using ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Commands.ModifyRecipe;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.ModifyRecipe;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Recipes;

public class ModifyRecipeCommandConverter : IToDomainConverter<(Guid, ModifyRecipeContract), ModifyRecipeCommand>
{
    public ModifyRecipeCommand ToDomain((Guid, ModifyRecipeContract) source)
    {
        (Guid id, ModifyRecipeContract? contract) = source;

        var ingredients = contract.Ingredients.Select(i =>
            new IngredientModification(
                i.Id is null ? null : new IngredientId(i.Id.Value),
                new ItemCategoryId(i.ItemCategoryId),
                i.QuantityType.ToEnum<IngredientQuantityType>(),
                new IngredientQuantity(i.Quantity)));

        var steps = contract.PreparationSteps.Select(s =>
            new PreparationStepModification(
                s.Id is null ? null : new PreparationStepId(s.Id.Value),
                new PreparationStepInstruction(s.Instruction),
                s.SortingIndex));

        var model = new RecipeModification(
            new RecipeId(id),
            new RecipeName(contract.Name),
            ingredients,
            steps);

        return new ModifyRecipeCommand(model);
    }
}