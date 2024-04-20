using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Manufacturers.Models;

public class ManufacturerBuilder : DomainTestBuilderBase<Manufacturer>
{
    public ManufacturerBuilder WithId(ManufacturerId id)
    {
        FillConstructorWith("id", id);
        return this;
    }

    public ManufacturerBuilder WithIsDeleted(bool isDeleted)
    {
        FillConstructorWith("isDeleted", isDeleted);
        return this;
    }
}