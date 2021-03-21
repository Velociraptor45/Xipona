using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Mocks
{
    public class StoreItemFactoryMock
    {
        private readonly Mock<IStoreItemFactory> mock;

        public StoreItemFactoryMock(Mock<IStoreItemFactory> mock)
        {
            this.mock = mock;
        }

        public StoreItemFactoryMock(Fixture fixture)
        {
            mock = fixture.Freeze<Mock<IStoreItemFactory>>();
        }

        public void SetupCreate(TemporaryItemCreation temporaryItemCreation, IStoreItemAvailability availability,
            IStoreItem returnValue)
        {
            mock
                .Setup(i => i.Create(
                    It.Is<TemporaryItemCreation>(obj => obj == temporaryItemCreation),
                    It.Is<IStoreItemAvailability>(av => av == availability)))
                .Returns(returnValue);
        }

        public void SetupCreate(ItemCreation itemCreation, IItemCategory itemCategory, IManufacturer manufacturer,
            IEnumerable<IStoreItemAvailability> availabilities, IStoreItem returnValue)
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