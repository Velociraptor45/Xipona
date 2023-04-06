namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

public interface IRecipeTag
{
    RecipeTagId Id { get; }
    string Name { get; }
}