using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Searches;

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