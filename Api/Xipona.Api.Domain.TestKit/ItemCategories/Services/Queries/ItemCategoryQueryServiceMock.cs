using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Queries;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Services.Queries;

public class ItemCategoryQueryServiceMock : Mock<IItemCategoryQueryService>
{
    public ItemCategoryQueryServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupGetAsync(ItemCategoryId itemCategoryId, IItemCategory returnValue)
    {
        Setup(m => m.GetAsync(itemCategoryId)).ReturnsAsync(returnValue);
    }
}