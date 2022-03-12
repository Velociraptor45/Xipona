using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;

namespace ShoppingList.Api.Domain.TestKit.Stores.Models.Factories
{
    public class StoreSectionFactoryMock : Mock<IStoreSectionFactory>
    {
        public StoreSectionFactoryMock(MockBehavior behavior) : base(behavior)
        {
        }
    }
}