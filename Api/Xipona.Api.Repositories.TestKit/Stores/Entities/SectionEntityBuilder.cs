using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Repositories.Stores.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.TestKit.Stores.Entities;

public class SectionEntityBuilder : TestBuilderBase<Section>
{
    public SectionEntityBuilder()
    {
        FillPropertyWith(s => s.Store, null);
    }

    public SectionEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(p => p.Id, id);
        return this;
    }

    public SectionEntityBuilder WithName(string name)
    {
        FillPropertyWith(p => p.Name, name);
        return this;
    }

    public SectionEntityBuilder WithStoreId(Guid storeId)
    {
        FillPropertyWith(p => p.StoreId, storeId);
        return this;
    }

    public SectionEntityBuilder WithSortIndex(int sortIndex)
    {
        FillPropertyWith(p => p.SortIndex, sortIndex);
        return this;
    }

    public SectionEntityBuilder WithIsDefaultSection(bool isDefaultSection)
    {
        FillPropertyWith(p => p.IsDefaultSection, isDefaultSection);
        return this;
    }

    public SectionEntityBuilder WithIsDeleted(bool isDeleted)
    {
        FillPropertyWith(p => p.IsDeleted, isDeleted);
        return this;
    }

    public SectionEntityBuilder WithStore(Store store)
    {
        FillPropertyWith(p => p.Store, store);
        return this;
    }
}