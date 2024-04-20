namespace ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;

public interface IRecipeTag
{
    RecipeTagId Id { get; }
    RecipeTagName Name { get; }
    DateTimeOffset CreatedAt { get; }
}