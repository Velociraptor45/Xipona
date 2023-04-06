namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models.Factories;

internal class RecipeTagFactory : IRecipeTagFactory
{
    public IRecipeTag Create(RecipeTagId id, string name)
    {
        return new RecipeTag(id, name);
    }
}