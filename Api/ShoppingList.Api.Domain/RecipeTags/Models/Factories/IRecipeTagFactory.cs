namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models.Factories;

public interface IRecipeTagFactory
{
    IRecipeTag Create(RecipeTagId id, string name);
    IRecipeTag CreateNew(string name);
}