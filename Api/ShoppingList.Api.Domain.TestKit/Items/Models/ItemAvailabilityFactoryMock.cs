using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ShoppingList.Api.Domain.TestKit.Items.Models;

public class ItemAvailabilityFactoryMock : Mock<IItemAvailabilityFactory>
{
    public ItemAvailabilityFactoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupCreate(StoreId storeId, Price price, SectionId sectionId,
        IItemAvailability returnValue)
    {
        Setup(i => i.Create(
                It.Is<StoreId>(id => id == storeId),
                It.Is<Price>(p => p == price),
                It.Is<SectionId>(id => id == sectionId)))
            .Returns(returnValue);
    }

    public void VerifyCreateOnce(StoreId storeId, Price price, SectionId sectionId)
    {
        Verify(i => i.Create(
                It.Is<StoreId>(id => id == storeId),
                It.Is<Price>(p => p == price),
                It.Is<SectionId>(id => id == sectionId)),
            Times.Once);
    }
}