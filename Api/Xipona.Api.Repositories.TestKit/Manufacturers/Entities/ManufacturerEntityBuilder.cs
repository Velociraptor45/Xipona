using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Repositories.Manufacturers.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.TestKit.Manufacturers.Entities;

public class ManufacturerEntityBuilder : TestBuilderBase<Manufacturer>
{
    public ManufacturerEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(p => p.Id, id);
        return this;
    }

    public ManufacturerEntityBuilder WithName(string name)
    {
        FillPropertyWith(p => p.Name, name);
        return this;
    }

    public ManufacturerEntityBuilder WithDeleted(bool deleted)
    {
        FillPropertyWith(p => p.Deleted, deleted);
        return this;
    }
}