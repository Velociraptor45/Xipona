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
        IngredientQuantity quantity)
    {
        await _validator.ValidateAsync(itemCategoryId);

        return new Ingredient(IngredientId.New, itemCategoryId, quantityType, quantity);
    }

    public async Task<IIngredient> CreateNewAsync(IngredientCreation creation)
    {
        await _validator.ValidateAsync(creation.ItemCategoryId);

        return new Ingredient(IngredientId.New, creation.ItemCategoryId, creation.QuantityType, creation.Quantity);
    }

    public IIngredient Create(IngredientId id, ItemCategoryId itemCategoryId, IngredientQuantityType quantityType,
        IngredientQuantity quantity)
    {
        return new Ingredient(id, itemCategoryId, quantityType, quantity);
    }
}