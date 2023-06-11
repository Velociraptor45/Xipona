using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public class Ingredient : IIngredient
{
    public Ingredient(IngredientId id, ItemCategoryId itemCategoryId, IngredientQuantityType quantityType,
        IngredientQuantity quantity, IngredientShoppingListProperties? shoppingListProperties)
    {
        Id = id;
        ItemCategoryId = itemCategoryId;
        QuantityType = quantityType;
        Quantity = quantity;
        ShoppingListProperties = shoppingListProperties;
    }

    public IngredientId Id { get; }
    public ItemCategoryId ItemCategoryId { get; }
    public IngredientQuantityType QuantityType { get; }
    public IngredientQuantity Quantity { get; }
    public IngredientShoppingListProperties? ShoppingListProperties { get; }
    public ItemId? DefaultItemId => ShoppingListProperties?.DefaultItemId;
    public ItemTypeId? DefaultItemTypeId => ShoppingListProperties?.DefaultItemTypeId;

    public async Task<IIngredient> ModifyAsync(IngredientModification modification, IValidator validator)
    {
        await validator.ValidateAsync(modification.ItemCategoryId);
        if (modification.ShoppingListProperties is not null)
            await validator.ValidateAsync(
                modification.ShoppingListProperties.DefaultItemId,
                modification.ShoppingListProperties.DefaultItemTypeId);

        return new Ingredient(Id, modification.ItemCategoryId, modification.QuantityType, modification.Quantity,
            modification.ShoppingListProperties);
    }

    public IIngredient RemoveDefaultItem()
    {
        return new Ingredient(
            Id,
            ItemCategoryId,
            QuantityType,
            Quantity,
            null);
    }

    public IIngredient ChangeDefaultItem(ItemId oldItemId, IItem newItem)
    {
        if (ShoppingListProperties is null || DefaultItemId != oldItemId)
            return this;

        if (DefaultItemTypeId is null)
        {
            return new Ingredient(
                Id,
                ItemCategoryId,
                QuantityType,
                Quantity,
                new IngredientShoppingListProperties(newItem.Id, null, ShoppingListProperties.DefaultStoreId,
                    ShoppingListProperties.AddToShoppingListByDefault));
        }

        if (!newItem.TryGetTypeWithPredecessor(DefaultItemTypeId.Value, out var itemType))
        {
            return RemoveDefaultItem();
        }

        return new Ingredient(
            Id,
            ItemCategoryId,
            QuantityType,
            Quantity,
            new IngredientShoppingListProperties(newItem.Id, itemType!.Id, ShoppingListProperties.DefaultStoreId,
                ShoppingListProperties.AddToShoppingListByDefault));
    }

    public IIngredient ChangeDefaultStore(IItem item)
    {
        if (ShoppingListProperties is null)
            throw new DomainException(new CannotChangeStoreOfIngredientWithoutShoppingListPropertiesReason(Id));

        var availability = DefaultItemTypeId is null
            ? item.Availabilities.First()
            : item.ItemTypes.First(t => t.Id == DefaultItemTypeId.Value).Availabilities.First();

        return new Ingredient(
            Id,
            ItemCategoryId,
            QuantityType,
            Quantity,
            new IngredientShoppingListProperties(
                DefaultItemId!.Value,
                DefaultItemTypeId,
                availability.StoreId,
                ShoppingListProperties.AddToShoppingListByDefault));
    }
}