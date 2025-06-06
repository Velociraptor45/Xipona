using Xipona.Api.Domain.Shared.Models;

namespace Xipona.Api.Domain.Stores.Models;

public record SectionName : Name
{
    public SectionName(string value) : base(value)
    {
    }
}