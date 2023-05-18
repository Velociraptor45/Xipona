using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;

public class IngredientMock : Mock<IIngredient>
{
    public IngredientMock(IIngredient ingredient, MockBehavior behavior) : base(behavior)
    {
        SetupId(ingredient.Id);
        SetupItemCategoryId(ingredient.ItemCategoryId);
        SetupQuantityType(ingredient.QuantityType);
        SetupQuantity(ingredient.Quantity);
        SetupDefaultItemId(ingredient.DefaultItemId);
        SetupDefaultItemTypeId(ingredient.DefaultItemTypeId);
        SetupShoppingListProperties(ingredient.ShoppingListProperties);
    }

    public void SetupId(IngredientId id)
    {
        Setup(m => m.Id).Returns(id);
    }

    public void SetupItemCategoryId(ItemCategoryId itemCategoryId)
    {
        Setup(m => m.ItemCategoryId).Returns(itemCategoryId);
    }

    public void SetupDefaultItemId(ItemId? defaultItemId)
    {
        Setup(m => m.DefaultItemId).Returns(defaultItemId);
    }

    public void SetupQuantityType(IngredientQuantityType quantityType)
    {
        Setup(m => m.QuantityType).Returns(quantityType);
    }

    public void SetupQuantity(IngredientQuantity quantity)
    {
        Setup(m => m.Quantity).Returns(quantity);
    }

    public void SetupShoppingListProperties(IngredientShoppingListProperties? shoppingListProperties)
    {
        Setup(m => m.ShoppingListProperties).Returns(shoppingListProperties);
    }

    public void SetupDefaultItemTypeId(ItemTypeId? defaultItemTypeId)
    {
        Setup(m => m.DefaultItemTypeId).Returns(defaultItemTypeId);
    }

    public void SetupModifyAsync(IngredientModification modification, IValidator validator, IIngredient returnValue)
    {
        Setup(m => m.ModifyAsync(modification, validator)).ReturnsAsync(returnValue);
    }

    public void VerifyModifyAsync(IngredientModification modification, IValidator validator, Func<Times> times)
    {
        Verify(m => m.ModifyAsync(modification, validator), times);
    }

    public void SetupChangingDefaultItem(ItemId oldItemId, IItem newItem, IIngredient returnValue)
    {
        Setup(m => m.ChangeDefaultItem(oldItemId, newItem)).Returns(returnValue);
    }

    public void VerifyChangingDefaultItem(ItemId oldItemId, IItem newItem, Func<Times> times)
    {
        Verify(m => m.ChangeDefaultItem(oldItemId, newItem), times);
    }
}