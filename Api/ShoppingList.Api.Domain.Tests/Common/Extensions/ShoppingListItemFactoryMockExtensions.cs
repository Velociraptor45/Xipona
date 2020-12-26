using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions
{
    public static class ShoppingListItemFactoryMockExtensions
    {
        public static void SetupCreate(this Mock<IShoppingListItemFactory> mock, IStoreItem storeItem,
            float price, bool isInBasket, float quantity, IShoppingListItem returnValue)
        {
            mock
                .Setup(instance => instance.Create(It.Is<IStoreItem>(i => i.Id == storeItem.Id),
                    It.Is<float>(p => p == price),
                    It.Is<bool>(b => b == isInBasket),
                    It.Is<float>(q => q == quantity)))
                .Returns(returnValue);
        }
    }
}