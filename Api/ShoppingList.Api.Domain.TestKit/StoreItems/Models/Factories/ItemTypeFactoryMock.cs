using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models.Factories;

public class ItemTypeFactoryMock : Mock<IItemTypeFactory>
{
    public ItemTypeFactoryMock(MockBehavior behavior) : base(behavior)
    {
    }
}