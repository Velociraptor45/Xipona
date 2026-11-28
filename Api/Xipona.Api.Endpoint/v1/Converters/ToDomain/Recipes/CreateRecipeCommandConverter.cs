using Xipona.Api.ApplicationServices.Recipes.Commands.CreateRecipe;
using Xipona.Api.Contracts.Recipes.Commands.CreateRecipe;
using Xipona.Api.Core.Converter;
using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.Common.Exceptions;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Recipes.Models;
using Xipona.Api.Domain.Recipes.Reasons;
using Xipona.Api.Domain.Recipes.Services.Creations;
using Xipona.Api.Domain.RecipeTags.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Endpoint.v1.Converters.ToDomain.Recipes;

public class CreateRecipeCommandConverter : IToDomainConverter<CreateRecipeContract, CreateRecipeCommand>
{
    public CreateRecipeCommand ToDomain(CreateRecipeContract source)
    {
        var steps = source.PreparationSteps.Select(s => new PreparationStepCreation(
            new PreparationStepInstruction(s.Instruction),
            s.SortingIndex));

        var ingredients = source.Ingredients.Select(i =>
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

            return new IngredientCreation(
                new ItemCategoryId(i.ItemCategoryId),
                i.QuantityType.ToEnum<IngredientQuantityType>(),
                new IngredientQuantity(i.Quantity),
                properties);
        });

        var tags = source.RecipeTagIds.Select(t => new RecipeTagId(t)).ToList();

        var creation = new RecipeCreation(
            new RecipeName(source.Name),
            new NumberOfServings(source.NumberOfServings),
            ingredients,
            steps,
            source.SideDishId is null ? null : new RecipeId(source.SideDishId.Value),
            tags);

        return new CreateRecipeCommand(creation);
    }
}