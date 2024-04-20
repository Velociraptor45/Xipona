using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.Xipona.Api.Domain.Shared.Validations;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Models.Factories;

public class IngredientFactory : IIngredientFactory
{
    private readonly IValidator _validator;

    public IngredientFactory(IValidator validator)
    {
        _validator = validator;
    }

    public async Task<IIngredient> CreateNewAsync(ItemCategoryId itemCategoryId, IngredientQuantityType quantityType,
        IngredientQuantity quantity, IngredientShoppingListProperties? shoppingListProperties)
    {
        await _validator.ValidateAsync(itemCategoryId);

        if (shoppingListProperties is not null)
            await _validator.ValidateAsync(
                shoppingListProperties.DefaultItemId,
                shoppingListProperties.DefaultItemTypeId);

        return new Ingredient(IngredientId.New, itemCategoryId, quantityType, quantity, shoppingListProperties);
    }

    public async Task<IIngredient> CreateNewAsync(IngredientCreation creation)
    {
        return await CreateNewAsync(creation.ItemCategoryId, creation.QuantityType, creation.Quantity,
            creation.ShoppingListProperties);
    }

    public IIngredient Create(IngredientId id, ItemCategoryId itemCategoryId, IngredientQuantityType quantityType,
        IngredientQuantity quantity, IngredientShoppingListProperties? shoppingListProperties)
    {
        return new Ingredient(id, itemCategoryId, quantityType, quantity, shoppingListProperties);
    }
}