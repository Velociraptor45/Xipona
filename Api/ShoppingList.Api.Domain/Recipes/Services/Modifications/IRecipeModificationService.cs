namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;

public interface IRecipeModificationService
{
    Task ModifyAsync(RecipeModification modification);
}