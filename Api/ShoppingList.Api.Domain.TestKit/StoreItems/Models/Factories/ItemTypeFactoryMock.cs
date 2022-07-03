using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models.Factories;

public class ItemTypeFactoryMock : Mock<IItemTypeFactory>
{
    public ItemTypeFactoryMock(MockBehavior behavior) : base(behavior)
    {
    }
}