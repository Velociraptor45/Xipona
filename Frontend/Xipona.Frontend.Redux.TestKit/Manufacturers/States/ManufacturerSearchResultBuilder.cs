using ProjectHermes.Xipona.Frontend.Redux.Manufacturers.States;
using ProjectHermes.Xipona.Frontend.TestTools;
using System;

namespace ProjectHermes.Xipona.Frontend.Redux.TestKit.Manufacturers.States;
public class ManufacturerSearchResultBuilder : TestBuilderBase<ManufacturerSearchResult>
{
    public ManufacturerSearchResultBuilder WithId(Guid id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public ManufacturerSearchResultBuilder WithName(string name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }
}