using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Creations;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Services.Creations;

public class ItemCategoryCreationServiceMock : Mock<IItemCategoryCreationService>
{
    public ItemCategoryCreationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupCreateAsync(ItemCategoryName name, IItemCategory returnValue)
    {
        Setup(s => s.CreateAsync(name)).ReturnsAsync(returnValue);
    }

    public void VerifyCreateAsync(ItemCategoryName name, Func<Times> times)
    {
        Verify(s => s.CreateAsync(name), times);
    }
}