using Xipona.Api.Contracts.ItemCategories.Queries;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.ItemCategories.States;

namespace Xipona.Frontend.Infrastructure.Converters.ItemCategories.ToDomain;

public class ItemCategorySearchResultConverter :
    IToDomainConverter<ItemCategorySearchResultContract, ItemCategorySearchResult>
{
    public ItemCategorySearchResult ToDomain(ItemCategorySearchResultContract source)
    {
        return new ItemCategorySearchResult(source.Id, source.Name);
    }
}