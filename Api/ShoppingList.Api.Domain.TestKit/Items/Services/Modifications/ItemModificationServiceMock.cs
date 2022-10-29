using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Modifications;

public class ItemModificationServiceMock : Mock<IItemModificationService>
{
    public ItemModificationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }
}