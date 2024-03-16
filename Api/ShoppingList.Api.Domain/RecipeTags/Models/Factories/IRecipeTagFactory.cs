namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models.Factories;

public interface IRecipeTagFactory
{
    IRecipeTag Create(RecipeTagId id, string name, DateTimeOffset createdAt);

    IRecipeTag CreateNew(string name);
}