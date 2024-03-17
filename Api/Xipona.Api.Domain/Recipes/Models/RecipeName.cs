using ProjectHermes.Xipona.Api.Domain.Shared.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Models;
public record RecipeName : Name
{
    public RecipeName(string value) : base(value)
    {
    }
}