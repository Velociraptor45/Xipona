using ProjectHermes.Xipona.Api.Domain.Shared.Models;

namespace ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;

public record RecipeTagName : Name
{
    public RecipeTagName(string value) : base(value)
    {
    }
}