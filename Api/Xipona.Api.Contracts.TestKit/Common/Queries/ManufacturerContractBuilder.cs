using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Core.TestKit;
using System;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.Common.Queries;
public class ManufacturerContractBuilder : TestBuilderBase<ManufacturerContract>
{
    public ManufacturerContractBuilder WithId(Guid id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public ManufacturerContractBuilder WithName(string name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }

    public ManufacturerContractBuilder WithoutName()
    {
        return WithName(null);
    }

    public ManufacturerContractBuilder WithIsDeleted(bool isDeleted)
    {
        FillConstructorWith(nameof(isDeleted), isDeleted);
        return this;
    }
}