using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;

public interface IRecipeModificationService
{
    Task ModifyAsync(RecipeModification modification);

    Task RemoveDefaultItemAsync(ItemId itemId);

    Task ModifyIngredientsAfterItemUpdateAsync(ItemId oldItemId, IItem newItem);
}