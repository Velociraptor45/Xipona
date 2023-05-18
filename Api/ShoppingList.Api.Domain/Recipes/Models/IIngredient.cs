using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public interface IIngredient
{
    IngredientId Id { get; }
    ItemCategoryId ItemCategoryId { get; }
    IngredientQuantityType QuantityType { get; }
    IngredientQuantity Quantity { get; }
    ItemId? DefaultItemId { get; }
    ItemTypeId? DefaultItemTypeId { get; }
    IngredientShoppingListProperties? ShoppingListProperties { get; }

    Task<IIngredient> ModifyAsync(IngredientModification modification, IValidator validator);

    IIngredient RemoveDefaultItem();
}