using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;

public interface IIngredientFactory
{
    Task<IIngredient> CreateNewAsync(IngredientCreation creation);

    Task<IIngredient> CreateNewAsync(ItemCategoryId itemCategoryId, IngredientQuantityType quantityType,
        IngredientQuantity quantity, ItemId? defaultItemId, ItemTypeId? defaultItemTypeId);

    IIngredient Create(IngredientId id, ItemCategoryId itemCategoryId, IngredientQuantityType quantityType,
        IngredientQuantity quantity, ItemId? defaultItemId, ItemTypeId? defaultItemTypeId);
}