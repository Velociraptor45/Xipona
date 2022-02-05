using Moq;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models;

public class StoreItemAvailabilityFactoryMock : Mock<IStoreItemAvailabilityFactory>
{
    public StoreItemAvailabilityFactoryMock(MockBehavior behavior) : base(behavior)

    {
    }

    public void SetupCreate(StoreId storeId, float price, SectionId sectionId,
        IStoreItemAvailability returnValue)
    {
        Setup(i => i.Create(
                It.Is<StoreId>(id => id == storeId),
                It.Is<float>(p => p == price),
                It.Is<SectionId>(id => id == sectionId)))
            .Returns(returnValue);
    }

    public void VerifyCreateOnce(StoreId storeId, float price, SectionId sectionId)
    {
        Verify(i => i.Create(
                It.Is<StoreId>(id => id == storeId),
                It.Is<float>(p => p == price),
                It.Is<SectionId>(id => id == sectionId)),
            Times.Once);
    }
}