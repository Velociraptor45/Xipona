using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Modifications;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Models;

public class ItemCategoryMock : Mock<IItemCategory>
{
    public ItemCategoryMock(IItemCategory itemCategory, MockBehavior behavior) : base(behavior)
    {
        SetupId(itemCategory.Id);
    }

    public void SetupId(ItemCategoryId returnValue)
    {
        Setup(i => i.Id)
            .Returns(returnValue);
    }

    public void VerifyDeleteOnce()
    {
        Verify(i => i.Delete(), Times.Once);
    }

    public void VerifyModify(ItemCategoryModification modification, Func<Times> times)
    {
        Verify(m => m.Modify(modification));
    }

    public void SetupModify(ItemCategoryModification modification)
    {
        Setup(m => m.Modify(modification));
    }

    public void SetupDelete()
    {
        Setup(m => m.Delete());
    }
}