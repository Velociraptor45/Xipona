using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Mocks
{
    public class ShoppingListFactoryMock
    {
        private readonly Mock<IShoppingListFactory> mock;

        public ShoppingListFactoryMock(Mock<IShoppingListFactory> mock)
        {
            this.mock = mock;
        }

        public ShoppingListFactoryMock(Fixture fixture)
        {
            mock = fixture.Freeze<Mock<IShoppingListFactory>>();
        }

        public void SetupCreate(IShoppingListStore store, IEnumerable<IShoppingListSection> sections,
            DateTime? completionDate, IShoppingList returnValue)
        {
            mock.Setup(i => i.Create(
                    It.Is<IShoppingListStore>(s => s == store),
                    It.Is<IEnumerable<IShoppingListSection>>(sec => sec.SequenceEqual(sections)),
                    It.Is<DateTime?>(date => date == completionDate)))
                .Returns(returnValue);
        }
    }
}