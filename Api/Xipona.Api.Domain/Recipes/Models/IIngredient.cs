using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Recipes.Services.Modifications;
using Xipona.Api.Domain.Shared.Validations;

namespace Xipona.Api.Domain.Recipes.Models;

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

    IIngredient ChangeDefaultItem(ItemId oldItemId, IItem newItem);

    IIngredient ChangeDefaultStore(IItem item);

    IIngredient ModifyAfterAvailabilitiesChanged(IEnumerable<ItemAvailability> newAvailabilities);
    IIngredient ReplaceDefaultItem(ItemId newItemId, ItemTypeId? newItemTypeId);
}