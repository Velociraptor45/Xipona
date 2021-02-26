using Moq;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions
{
    public static class StoreItemSectionReadRepositoryMockExtensions
    {
        public static void SetupFindByAsync(this Mock<IStoreItemSectionReadRepository> mock,
            IEnumerable<StoreItemSectionId> sectionIds, IEnumerable<IStoreItemSection> returnValue)
        {
            mock.Setup(i => i.FindByAsync(
                    It.Is<IEnumerable<StoreItemSectionId>>(ids => ids.SequenceEqual(sectionIds)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);
        }

        public static void SetupFindByAsync(this Mock<IStoreItemSectionReadRepository> mock,
            StoreItemSectionId sectionId, IStoreItemSection returnValue)
        {
            mock.Setup(i => i.FindByAsync(
                    It.Is<StoreItemSectionId>(id => id == sectionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);
        }
    }
}