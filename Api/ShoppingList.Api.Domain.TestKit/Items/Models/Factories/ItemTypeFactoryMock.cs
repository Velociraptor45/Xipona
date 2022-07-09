using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models.Factories;

public class ItemTypeFactoryMock : Mock<IItemTypeFactory>
{
    public ItemTypeFactoryMock(MockBehavior behavior) : base(behavior)
    {
    }
}