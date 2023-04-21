using ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Commands.ModifyRecipe;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.ModifyRecipe;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Recipes;

public class ModifyRecipeCommandConverter : IToDomainConverter<(Guid, ModifyRecipeContract), ModifyRecipeCommand>
{
    public ModifyRecipeCommand ToDomain((Guid, ModifyRecipeContract) source)
    {
        (Guid id, ModifyRecipeContract? contract) = source;

        var ingredients = contract.Ingredients.Select(i =>
        {
            IngredientShoppingListProperties? properties = null;
            if (i.DefaultItemId.HasValue)
            {
                properties = new IngredientShoppingListProperties(
                    new ItemId(i.DefaultItemId.Value),
                    i.DefaultItemTypeId is null ? null : new ItemTypeId(i.DefaultItemTypeId.Value),
                    i.DefaultStoreId.HasValue
                        ? new StoreId(i.DefaultStoreId.Value)
                        : throw new DomainException(new DefaultIngredientItemHasToHaveDefaultStoreReason()),
                    i.AddToShoppingListByDefault ?? false);
            }

            return new IngredientModification(
                i.Id is null ? null : new IngredientId(i.Id.Value),
                new ItemCategoryId(i.ItemCategoryId),
                i.QuantityType.ToEnum<IngredientQuantityType>(),
                new IngredientQuantity(i.Quantity),
                properties
            );
        });

        var steps = contract.PreparationSteps.Select(s =>
            new PreparationStepModification(
                s.Id is null ? null : new PreparationStepId(s.Id.Value),
                new PreparationStepInstruction(s.Instruction),
                s.SortingIndex));

        var tags = contract.RecipeTagIds.Select(t => new RecipeTagId(t));

        var model = new RecipeModification(
            new RecipeId(id),
            new RecipeName(contract.Name),
            ingredients,
            steps,
            tags);

        return new ModifyRecipeCommand(model);
    }
}