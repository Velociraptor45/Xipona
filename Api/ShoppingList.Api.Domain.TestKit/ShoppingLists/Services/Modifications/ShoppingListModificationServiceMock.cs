using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Services.Modifications;

public class ShoppingListModificationServiceMock : Mock<IShoppingListModificationService>
{
    public ShoppingListModificationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupRemoveSectionAsync(SectionId sectionId)
    {
        Setup(m => m.RemoveSectionAsync(sectionId)).Returns(Task.CompletedTask);
    }
}