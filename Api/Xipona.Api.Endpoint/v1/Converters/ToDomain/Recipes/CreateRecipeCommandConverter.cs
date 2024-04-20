using ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Commands.CreateRecipe;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Commands.CreateRecipe;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Reasons;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.Recipes;

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
            tags);

        return new CreateRecipeCommand(creation);
    }
}