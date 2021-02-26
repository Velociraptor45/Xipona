using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions
{
    public static class ShoppingListFactoryMockExtensions
    {
        public static void SetupCreate(this Mock<IShoppingListFactory> mock,
            IShoppingListStore store, IEnumerable<IShoppingListSection> sections, DateTime? completionDate, IShoppingList returnValue)
        {
            mock.Setup(i => i.Create(
                    It.Is<IShoppingListStore>(s => s == store),
                    It.Is<IEnumerable<IShoppingListSection>>(sec => sec.SequenceEqual(sections)),
                    It.Is<DateTime?>(date => date == completionDate)))
                .Returns(returnValue);
        }
    }
}