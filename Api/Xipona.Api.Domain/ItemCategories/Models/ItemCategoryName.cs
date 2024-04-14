using ProjectHermes.Xipona.Api.Domain.Shared.Models;

namespace ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;

public record ItemCategoryName : Name
{
    public ItemCategoryName(string value) : base(value)
    {
    }
}