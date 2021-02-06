using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
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
    }
}