using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Mocks
{
    public class StoreItemSectionReadRepositoryMock
    {
        private readonly Mock<IStoreItemSectionReadRepository> mock;

        public StoreItemSectionReadRepositoryMock(Mock<IStoreItemSectionReadRepository> mock)
        {
            this.mock = mock;
        }

        public StoreItemSectionReadRepositoryMock(Fixture fixture)
        {
            mock = fixture.Freeze<Mock<IStoreItemSectionReadRepository>>();
        }

        public void SetupFindByAsync(IEnumerable<StoreItemSectionId> sectionIds,
            IEnumerable<IStoreItemSection> returnValue)
        {
            mock.Setup(i => i.FindByAsync(
                    It.Is<IEnumerable<StoreItemSectionId>>(ids => ids.SequenceEqual(sectionIds)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);
        }

        public void SetupFindByAsync(StoreItemSectionId sectionId, IStoreItemSection returnValue)
        {
            mock.Setup(i => i.FindByAsync(
                    It.Is<StoreItemSectionId>(id => id == sectionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);
        }
    }
}