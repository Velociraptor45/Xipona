using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public class Ingredients : IEnumerable<IIngredient>
{
    private readonly IIngredientFactory _ingredientFactory;
    private readonly IDictionary<IngredientId, IIngredient> _ingredients;

    public Ingredients(IEnumerable<IIngredient> ingredients, IIngredientFactory ingredientFactory)
    {
        _ingredientFactory = ingredientFactory;
        _ingredients = ingredients.ToDictionary(i => i.Id);
    }

    public IReadOnlyCollection<IIngredient> AsReadOnly()
    {
        return _ingredients.Values.ToList().AsReadOnly();
    }

    public IEnumerator<IIngredient> GetEnumerator()
    {
        return _ingredients.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public async Task ModifyManyAsync(IEnumerable<IngredientModification> modifications, IValidator validator)
    {
        var modificationsList = modifications.ToList();

        var ingredientsToModify = modificationsList.Where(s => s.Id.HasValue)
            .ToDictionary(modification => modification.Id!.Value);
        var ingredientsToCreate = modificationsList.Where(s => !s.Id.HasValue);
        var ingredientIdsToDelete = _ingredients.Keys.Where(id => !ingredientsToModify.ContainsKey(id));
        var newIngredients = new List<IIngredient>();
        foreach (var modification in ingredientsToCreate)
        {
            var newIngredient = await _ingredientFactory.CreateNewAsync(modification.ItemCategoryId,
                modification.QuantityType, modification.Quantity, modification.ShoppingListProperties);
            newIngredients.Add(newIngredient);
        }

        foreach (var ingredientId in ingredientIdsToDelete)
        {
            Remove(ingredientId);
        }

        foreach (var ingredient in ingredientsToModify.Values)
        {
            await ModifyAsync(ingredient, validator);
        }

        AddMany(newIngredients);
    }

    private async Task ModifyAsync(IngredientModification modification, IValidator validator)
    {
        if (!modification.Id.HasValue)
            throw new ArgumentException("Id mustn't be null.");

        if (!_ingredients.TryGetValue(modification.Id.Value, out var ingredient))
        {
            throw new DomainException(new IngredientNotFoundReason(modification.Id.Value));
        }

        var modifiedType = await ingredient.ModifyAsync(modification, validator);
        _ingredients[modifiedType.Id] = modifiedType;
    }

    private void Remove(IngredientId id)
    {
        _ingredients.Remove(id);
    }

    private void AddMany(IEnumerable<IIngredient> types)
    {
        foreach (var type in types)
        {
            Add(type);
        }
    }

    private void Add(IIngredient ingredient)
    {
        _ingredients.Add(ingredient.Id, ingredient);
    }

    public void RemoveDefaultItem(ItemId defaultItemId, ItemTypeId? itemTypeId)
    {
        var ingredientsWithItem = _ingredients.Values
            .Where(i => i.DefaultItemId == defaultItemId
                        && (itemTypeId is null || i.DefaultItemTypeId == itemTypeId));
        foreach (var ingredient in ingredientsWithItem)
        {
            _ingredients[ingredient.Id] = ingredient.RemoveDefaultItem();
        }
    }

    public void ModifyAfterItemUpdate(ItemId oldItemId, IItem newItem)
    {
        var ingredientsWithItem = _ingredients.Values.Where(i => i.DefaultItemId == oldItemId);
        foreach (var ingredient in ingredientsWithItem)
        {
            _ingredients[ingredient.Id] = ingredient.ChangeDefaultItem(oldItemId, newItem);
        }
    }

    public void ModifyAfterAvailabilityWasDeleted(ItemId itemId, ItemTypeId? itemTypeId, IItem item,
        StoreId deletedAvailabilityStoreId)
    {
        var ingredientsWithItem = _ingredients.Values
            .Where(i => i.DefaultItemId == itemId
                        && i.DefaultItemTypeId == itemTypeId
                        && i.ShoppingListProperties?.DefaultStoreId == deletedAvailabilityStoreId);

        foreach (var ingredient in ingredientsWithItem)
        {
            _ingredients[ingredient.Id] = ingredient.ChangeDefaultStore(item);
        }
    }

    public void RemoveIngredientsOfItemCategory(ItemCategoryId itemCategoryId)
    {
        var ingredients = _ingredients.Values.Where(i => i.ItemCategoryId == itemCategoryId);
        foreach (var ingredient in ingredients)
        {
            _ingredients.Remove(ingredient.Id);
        }
    }

    public void ModifyAfterAvailabilitiesChanged(ItemId itemId, ItemTypeId? itemTypeId,
        IEnumerable<IItemAvailability> oldAvailabilities, IEnumerable<IItemAvailability> newAvailabilities)
    {
        var ingredientsWithItem = _ingredients.Values
            .Where(i => i.DefaultItemId == itemId && i.DefaultItemTypeId == itemTypeId);

        foreach (var ingredient in ingredientsWithItem)
        {
            _ingredients[ingredient.Id] = ingredient.ModifyAfterAvailabilitiesChanged(oldAvailabilities, newAvailabilities);
        }
    }
}