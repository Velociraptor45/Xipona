using Moq;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models
{
    public class StoreItemFactoryMock : Mock<IStoreItemFactory>
    {
        public StoreItemFactoryMock(MockBehavior behavior) : base(behavior)
        {
        }

        public void SetupCreate(TemporaryItemCreation temporaryItemCreation, IStoreItem returnValue)
        {
            Setup(i => i.Create(
                    It.Is<TemporaryItemCreation>(obj => obj == temporaryItemCreation)))
                .Returns(returnValue);
        }

        public void SetupCreate(ItemCreation itemCreation, IStoreItem returnValue)
        {
            Setup(i => i.Create(
                    It.Is<ItemCreation>(c => c == itemCreation)))
                .Returns(returnValue);
        }
    }
}