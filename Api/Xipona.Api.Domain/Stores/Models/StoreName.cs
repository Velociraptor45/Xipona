using ProjectHermes.Xipona.Api.Domain.Shared.Models;

namespace ProjectHermes.Xipona.Api.Domain.Stores.Models;

public record StoreName : Name
{
    public StoreName(string value) : base(value)
    {
    }
}