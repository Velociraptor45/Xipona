using Xipona.Frontend.Redux.ItemCategories.States;
using Xipona.Frontend.TestTools;
using System;

namespace Xipona.Frontend.Redux.TestKit.ItemCategories.States;
public class ItemCategorySearchResultBuilder : TestBuilderBase<ItemCategorySearchResult>
{
    public ItemCategorySearchResultBuilder WithId(Guid id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public ItemCategorySearchResultBuilder WithName(string name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }
}