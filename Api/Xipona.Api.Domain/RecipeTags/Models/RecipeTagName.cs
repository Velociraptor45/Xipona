using Xipona.Api.Domain.Shared.Models;

namespace Xipona.Api.Domain.RecipeTags.Models;

public record RecipeTagName : Name
{
    public RecipeTagName(string value) : base(value)
    {
    }
}