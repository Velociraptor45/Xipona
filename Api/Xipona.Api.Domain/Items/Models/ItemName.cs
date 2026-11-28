using Xipona.Api.Domain.Shared.Models;

namespace Xipona.Api.Domain.Items.Models;
public record ItemName : Name
{
    public ItemName(string value) : base(value)
    {
    }
}