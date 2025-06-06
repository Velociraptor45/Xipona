using Xipona.Api.Domain.Shared.Models;

namespace Xipona.Api.Domain.Items.Models;

public record ItemTypeName : Name
{
    public ItemTypeName(string value) : base(value)
    {
    }
}