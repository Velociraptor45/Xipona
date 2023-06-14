using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;

public class IngredientFactory : IIngredientFactory
{
    private readonly IValidator _validator;

    public IngredientFactory(Func<CancellationToken, IValidator> validatorDelegate, CancellationToken cancellationToken)
    {
        _validator = validatorDelegate(cancellationToken);
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