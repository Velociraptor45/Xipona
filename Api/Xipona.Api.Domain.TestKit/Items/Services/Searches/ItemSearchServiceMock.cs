using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Searches;

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