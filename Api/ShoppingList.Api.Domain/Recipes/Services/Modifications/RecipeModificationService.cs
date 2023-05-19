using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;

public class RecipeModificationService : IRecipeModificationService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IValidator _validator;

    public RecipeModificationService(
        Func<CancellationToken, IRecipeRepository> recipeRepositoryDelegate,
        Func<CancellationToken, IValidator> validatorDelegate,
        CancellationToken cancellationToken)
    {
        _recipeRepository = recipeRepositoryDelegate(cancellationToken);
        _validator = validatorDelegate(cancellationToken);
    }

    public async Task ModifyAsync(RecipeModification modification)
    {
        var recipe = await _recipeRepository.FindByAsync(modification.Id);

        if (recipe is null)
            throw new DomainException(new RecipeNotFoundReason(modification.Id));

        await recipe.ModifyAsync(modification, _validator);

        await _recipeRepository.StoreAsync(recipe);
    }

    public async Task RemoveDefaultItemAsync(ItemId itemId)
    {
        var recipes = await _recipeRepository.FindByAsync(itemId);

        foreach (var recipe in recipes)
        {
            recipe.RemoveDefaultItem(itemId);
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
}