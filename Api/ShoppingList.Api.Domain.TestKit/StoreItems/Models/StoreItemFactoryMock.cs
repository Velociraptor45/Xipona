using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models;

public class StoreItemFactoryMock : Mock<IItemFactory>
{
    public StoreItemFactoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupCreate(TemporaryItemCreation temporaryItemCreation, IItem returnValue)
    {
        Setup(i => i.Create(
                It.Is<TemporaryItemCreation>(obj => obj == temporaryItemCreation)))
            .Returns(returnValue);
    }

    public void SetupCreate(ItemCreation itemCreation, IItem returnValue)
    {
        Setup(i => i.Create(
                It.Is<ItemCreation>(c => c == itemCreation)))
            .Returns(returnValue);
    }
}