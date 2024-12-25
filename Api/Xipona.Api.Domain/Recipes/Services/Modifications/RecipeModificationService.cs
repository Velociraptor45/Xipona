using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.Items.Reasons;
using ProjectHermes.Xipona.Api.Domain.Recipes.Ports;
using ProjectHermes.Xipona.Api.Domain.Recipes.Reasons;
using ProjectHermes.Xipona.Api.Domain.Shared.Validations;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Services.Modifications;

public class RecipeModificationService : IRecipeModificationService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IValidator _validator;
    private readonly IItemRepository _itemRepository;

    public RecipeModificationService(IRecipeRepository recipeRepository, IItemRepository itemRepository,
        IValidator validator)
    {
        _recipeRepository = recipeRepository;
        _itemRepository = itemRepository;
        _validator = validator;
    }

    public async Task ModifyAsync(RecipeModification modification)
    {
        if (modification.SideDishId is not null)
        {
            var sideDishExists = await _recipeRepository.Exists(modification.SideDishId.Value);
            if (!sideDishExists)
                throw new DomainException(new RecipeNotFoundReason(modification.SideDishId.Value));
        }

        var recipe = await _recipeRepository.FindByAsync(modification.Id);

        if (recipe is null)
            throw new DomainException(new RecipeNotFoundReason(modification.Id));

        await recipe.ModifyAsync(modification, _validator);

        await _recipeRepository.StoreAsync(recipe);
    }

    public async Task RemoveDefaultItemAsync(ItemId itemId, ItemTypeId? itemTypeId)
    {
        var recipes = await _recipeRepository.FindByAsync(itemId);

        foreach (var recipe in recipes)
        {
            recipe.RemoveDefaultItem(itemId, itemTypeId);
            await _recipeRepository.StoreAsync(recipe);
        }
    }

    public async Task ModifyIngredientsAfterItemUpdateAsync(ItemId oldItemId, IItem newItem)
    {
        var recipes = await _recipeRepository.FindByAsync(oldItemId);

        foreach (var recipe in recipes)
        {
            recipe.ModifyIngredientsAfterItemUpdate(oldItemId, newItem);
            await _recipeRepository.StoreAsync(recipe);
        }
    }

    public async Task ModifyIngredientsAfterAvailabilitiesChangedAsync(ItemId itemId, ItemTypeId? itemTypeId,
        IEnumerable<ItemAvailability> newAvailabilities)
    {
        var recipes = await _recipeRepository.FindByAsync(itemId, itemTypeId);
        var item = await _itemRepository.FindActiveByAsync(itemId);
        if (item is null)
            throw new DomainException(new ItemNotFoundReason(itemId));

        var newAvailabilitiesList = newAvailabilities.ToList();

        foreach (var recipe in recipes)
        {
            recipe.ModifyIngredientsAfterAvailabilitiesChanged(itemId, itemTypeId, newAvailabilitiesList);
            await _recipeRepository.StoreAsync(recipe);
        }
    }

    public async Task ModifyIngredientsAfterAvailabilityWasDeletedAsync(ItemId itemId, ItemTypeId? itemTypeId,
        StoreId deletedAvailabilityStoreId)
    {
        var recipes = await _recipeRepository.FindByAsync(itemId, itemTypeId, deletedAvailabilityStoreId);
        var item = await _itemRepository.FindActiveByAsync(itemId);
        if (item is null)
            throw new DomainException(new ItemNotFoundReason(itemId));

        foreach (var recipe in recipes)
        {
            recipe.ModifyIngredientsAfterAvailabilityWasDeleted(itemId, itemTypeId, item, deletedAvailabilityStoreId);
            await _recipeRepository.StoreAsync(recipe);
        }
    }

    public async Task RemoveIngredientsOfItemCategoryAsync(ItemCategoryId itemCategoryId)
    {
        var recipes = (await _recipeRepository.FindByAsync(itemCategoryId)).ToList();

        foreach (var recipe in recipes)
        {
            recipe.RemoveIngredientsOfItemCategory(itemCategoryId);
            await _recipeRepository.StoreAsync(recipe);
        }
    }
}