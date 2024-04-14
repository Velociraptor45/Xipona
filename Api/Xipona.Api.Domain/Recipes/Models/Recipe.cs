﻿using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.Shared.Validations;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Models;

public class Recipe : AggregateRoot, IRecipe
{
    private readonly Ingredients _ingredients;
    private readonly PreparationSteps _steps;
    private readonly RecipeTags _tags;

    public Recipe(RecipeId id, RecipeName name, NumberOfServings numberOfServings, Ingredients ingredients,
        PreparationSteps steps, RecipeTags tags, DateTimeOffset createdAt)
    {
        _ingredients = ingredients;
        _steps = steps;
        _tags = tags;
        Id = id;
        Name = name;
        NumberOfServings = numberOfServings;
        CreatedAt = createdAt;
    }

    public RecipeId Id { get; }
    public RecipeName Name { get; private set; }
    public NumberOfServings NumberOfServings { get; private set; }
    public DateTimeOffset CreatedAt { get; }
    public IReadOnlyCollection<IIngredient> Ingredients => _ingredients.AsReadOnly();
    public IReadOnlyCollection<IPreparationStep> PreparationSteps => _steps.AsReadOnly();
    public IReadOnlyCollection<RecipeTagId> Tags => _tags.AsReadOnly();

    public async Task ModifyAsync(RecipeModification modification, IValidator validator)
    {
        Name = modification.Name;
        NumberOfServings = modification.NumberOfServings;
        await _ingredients.ModifyManyAsync(modification.IngredientModifications, validator);
        _steps.ModifyMany(modification.PreparationStepModifications);
        await _tags.ModifyAsync(validator, modification.RecipeTagIds);
    }

    public void RemoveDefaultItem(ItemId defaultItemId, ItemTypeId? itemTypeId)
    {
        _ingredients.RemoveDefaultItem(defaultItemId, itemTypeId);
    }

    public void ModifyIngredientsAfterItemUpdate(ItemId oldItemId, IItem newItem)
    {
        _ingredients.ModifyAfterItemUpdate(oldItemId, newItem);
    }

    public void ModifyIngredientsAfterAvailabilityWasDeleted(ItemId itemId, ItemTypeId? itemTypeId, IItem item,
        StoreId deletedAvailabilityStoreId)
    {
        _ingredients.ModifyAfterAvailabilityWasDeleted(itemId, itemTypeId, item, deletedAvailabilityStoreId);
    }

    public void ModifyIngredientsAfterAvailabilitiesChanged(ItemId itemId, ItemTypeId? itemTypeId,
        IEnumerable<ItemAvailability> newAvailabilities)
    {
        _ingredients.ModifyAfterAvailabilitiesChanged(itemId, itemTypeId, newAvailabilities);
    }

    public void RemoveIngredientsOfItemCategory(ItemCategoryId itemCategoryId)
    {
        _ingredients.RemoveIngredientsOfItemCategory(itemCategoryId);
    }
}