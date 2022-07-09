using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models.Factories;

public class SectionFactoryMock : Mock<ISectionFactory>
{
    public SectionFactoryMock(MockBehavior behavior) : base(behavior)
    {
    }
}