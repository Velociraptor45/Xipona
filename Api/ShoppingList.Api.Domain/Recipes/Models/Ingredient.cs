using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public class Ingredient : IIngredient
{
    public Ingredient(IngredientId id, ItemCategoryId itemCategoryId, IngredientQuantityType quantityType,
        IngredientQuantity quantity, ItemId? defaultItemId, ItemTypeId? defaultItemTypeId)
    {
        Id = id;
        ItemCategoryId = itemCategoryId;
        QuantityType = quantityType;
        Quantity = quantity;
        DefaultItemId = defaultItemId;
        DefaultItemTypeId = defaultItemTypeId;

        if (DefaultItemId is null && DefaultItemTypeId.HasValue)
            throw new DomainException(new InvalidItemIdCombinationReason());
    }

    public IngredientId Id { get; }
    public ItemCategoryId ItemCategoryId { get; }
    public IngredientQuantityType QuantityType { get; }
    public IngredientQuantity Quantity { get; }
    public ItemId? DefaultItemId { get; }
    public ItemTypeId? DefaultItemTypeId { get; }

    public async Task<IIngredient> ModifyAsync(IngredientModification modification, IValidator validator)
    {
        await validator.ValidateAsync(modification.ItemCategoryId);
        if (modification.DefaultItemId.HasValue)
            await validator.ValidateAsync(modification.DefaultItemId.Value, modification.DefaultItemTypeId);

        return new Ingredient(Id, modification.ItemCategoryId, modification.QuantityType, modification.Quantity,
            modification.DefaultItemId, modification.DefaultItemTypeId);
    }

    public IIngredient RemoveDefaultItem()
    {
        return new Ingredient(
            IngredientId.New,
            ItemCategoryId,
            QuantityType,
            Quantity,
            null,
            null);
    }
}