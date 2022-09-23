using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
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
        IngredientQuantity quantity, ItemId? defaultItemId, ItemTypeId? defaultItemTypeId)
    {
        await _validator.ValidateAsync(itemCategoryId);

        return new Ingredient(IngredientId.New, itemCategoryId, quantityType, quantity, defaultItemId, defaultItemTypeId);
    }

    public async Task<IIngredient> CreateNewAsync(IngredientCreation creation)
    {
        await _validator.ValidateAsync(creation.ItemCategoryId);
        if (creation.DefaultItemId.HasValue)
            await _validator.ValidateAsync(creation.DefaultItemId.Value, creation.DefaultItemTypeId);

        return new Ingredient(IngredientId.New, creation.ItemCategoryId, creation.QuantityType, creation.Quantity,
            creation.DefaultItemId, creation.DefaultItemTypeId);
    }

    public IIngredient Create(IngredientId id, ItemCategoryId itemCategoryId, IngredientQuantityType quantityType,
        IngredientQuantity quantity, ItemId? defaultItemId, ItemTypeId? defaultItemTypeId)
    {
        return new Ingredient(id, itemCategoryId, quantityType, quantity, defaultItemId, defaultItemTypeId);
    }
}