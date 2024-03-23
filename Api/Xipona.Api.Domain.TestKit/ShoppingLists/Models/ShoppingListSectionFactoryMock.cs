using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Models;

public class ShoppingListSectionFactoryMock : Mock<IShoppingListSectionFactory>
{
    public ShoppingListSectionFactoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupCreateEmpty(SectionId sectionId, IShoppingListSection returnValue)
    {
        Setup(instance => instance.CreateEmpty(
                It.Is<SectionId>(id => id == sectionId)))
            .Returns(returnValue);
    }

    public void VerifyCreateEmptyOnce(SectionId sectionId)
    {
        Verify(i => i.CreateEmpty(
                It.Is<SectionId>(id => id == sectionId)),
            Times.Once);
    }
}