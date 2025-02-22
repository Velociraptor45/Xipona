using ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Commands.ModifyRecipe;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Commands.ModifyRecipe;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Reasons;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.Recipes;

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
            new NumberOfServings(contract.NumberOfServings),
            ingredients,
            steps,
            tags,
            contract.SideDishId is null ? null : new RecipeId(contract.SideDishId.Value));

        return new ModifyRecipeCommand(model);
    }
}