using ProjectHermes.Xipona.Api.Domain.Shared.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Models;
public record ItemName : Name
{
    public ItemName(string value) : base(value)
    {
    }
}