using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using DomainModels = ShoppingList.Api.Domain.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Models
{
    public class ShoppingListTests
    {
        [Fact]
        public void AddItem_WithStoreItemIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var commonFixture = new CommonFixture();
            var fixure = commonFixture.GetNewFixture();
            var shoppingList = fixure.Create<DomainModels.ShoppingList>();

            // Act
            Action action = () => shoppingList.AddItem(null, false, 20.4f);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void AddItem_WithItemIdIsAlreadyOnList_ShouldThrowItemAlreadyOnShoppingListException()
        {
            // Arrange
            var commonFixture = new CommonFixture();
            var shoppingListItemFixture = new ShoppingListItemFixture();

            var itemId = commonFixture.NextInt();
            var itemsAlreadyOnList = new List<ShoppingListItem>
            {
                shoppingListItemFixture.GetShoppingListItemWithId(new ShoppingListItemId(itemId)),
                shoppingListItemFixture.GetShoppingListItemWithId(new ShoppingListItemId(commonFixture.NextInt()))
            }.AsEnumerable();

            var fixure = commonFixture.GetNewFixture();
            fixure.Inject(itemsAlreadyOnList);
            var shoppingList = fixure.Create<DomainModels.ShoppingList>();
            var collidingItem = shoppingListItemFixture.GetStoreItemWithId(new StoreItemId(itemId));

            // Act
            Action action = () => shoppingList.AddItem(collidingItem, false, 20.4f);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ItemAlreadyOnShoppingListException>();
            }
        }
    }
}