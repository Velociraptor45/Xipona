using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Services.Searches;

namespace Xipona.Api.Domain.TestKit.Items.Services.Searches;

public class ItemSearchServiceMock : Mock<IItemSearchService>
{
    public ItemSearchServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupSearchAsync(ItemCategoryId itemCategoryId, IEnumerable<SearchItemByItemCategoryResult> returnValue)
    {
        Setup(m => m.SearchAsync(itemCategoryId)).ReturnsAsync(returnValue);
    }
}