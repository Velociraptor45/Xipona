using ProjectHermes.Xipona.Api.Domain.Shared.Models;

namespace ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

public record ManufacturerName : Name
{
    public ManufacturerName(string value) : base(value)
    {
    }
}