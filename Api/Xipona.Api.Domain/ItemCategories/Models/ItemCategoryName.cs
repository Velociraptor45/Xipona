using Xipona.Api.Domain.Shared.Models;

namespace Xipona.Api.Domain.ItemCategories.Models;

public record ItemCategoryName : Name
{
    public ItemCategoryName(string value) : base(value)
    {
    }
}