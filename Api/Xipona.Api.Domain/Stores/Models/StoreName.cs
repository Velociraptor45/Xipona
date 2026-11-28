using Xipona.Api.Domain.Shared.Models;

namespace Xipona.Api.Domain.Stores.Models;

public record StoreName : Name
{
    public StoreName(string value) : base(value)
    {
    }
}