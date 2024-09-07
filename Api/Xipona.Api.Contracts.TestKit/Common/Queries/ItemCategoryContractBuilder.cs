using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Core.TestKit;
using System;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.Common.Queries;
public class ItemCategoryContractBuilder : TestBuilderBase<ItemCategoryContract>
{
    public ItemCategoryContractBuilder WithId(Guid id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public ItemCategoryContractBuilder WithName(string name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }

    public ItemCategoryContractBuilder WithoutName()
    {
        return WithName(null);
    }

    public ItemCategoryContractBuilder WithIsDeleted(bool isDeleted)
    {
        FillConstructorWith(nameof(isDeleted), isDeleted);
        return this;
    }
}