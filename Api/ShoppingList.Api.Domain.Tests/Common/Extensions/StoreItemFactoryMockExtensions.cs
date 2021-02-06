using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions
{
    public static class StoreItemFactoryMockExtensions
    {
        public static void SetupCreate(this Mock<IStoreItemFactory> mock, TemporaryItemCreation temporaryItemCreation,
            IStoreItemAvailability availability, IStoreItem returnValue)
        {
            mock
                .Setup(i => i.Create(
                    It.Is<TemporaryItemCreation>(obj => obj == temporaryItemCreation),
                    It.Is<IStoreItemAvailability>(av => av == availability)))
                .Returns(returnValue);
        }

        public static void SetupCreate(this Mock<IStoreItemFactory> mock, ItemCreation itemCreation,
            IItemCategory itemCategory, IManufacturer manufacturer, IEnumerable<IStoreItemAvailability> availabilities,
            IStoreItem returnValue)
        {
            mock
                .Setup(i => i.Create(
                    It.Is<ItemCreation>(c => c == itemCreation),
                    It.Is<IItemCategory>(cat => cat == itemCategory),
                    It.Is<IManufacturer>(man => man == manufacturer),
                    It.Is<IEnumerable<IStoreItemAvailability>>(list => list.SequenceEqual(availabilities))))
                .Returns(returnValue);
        }
    }
}