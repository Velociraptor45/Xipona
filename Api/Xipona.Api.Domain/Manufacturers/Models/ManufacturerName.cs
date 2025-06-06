using Xipona.Api.Domain.Shared.Models;

namespace Xipona.Api.Domain.Manufacturers.Models;

public record ManufacturerName : Name
{
    public ManufacturerName(string value) : base(value)
    {
    }
}