using Xipona.Api.Core.TestKit;
using Xipona.Api.Repositories.ItemCategories.Entities;

namespace Xipona.Api.Repositories.TestKit.ItemCategories.Entities;

public class ItemCategoryEntityBuilder : TestBuilderBase<ItemCategory>
{
    public ItemCategoryEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(p => p.Id, id);
        return this;
    }

    public ItemCategoryEntityBuilder WithName(string name)
    {
        FillPropertyWith(p => p.Name, name);
        return this;
    }

    public ItemCategoryEntityBuilder WithDeleted(bool deleted)
    {
        FillPropertyWith(p => p.Deleted, deleted);
        return this;
    }
}