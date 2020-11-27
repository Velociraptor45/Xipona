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
            var fixure = CommonFixture.GetNewFixture();
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
            var itemId = CommonFixture.NextInt();
            var itemsAlreadyOnList = new List<ShoppingListItem>
            {
                ModelFixture.GetShoppingListItemWithId(new ShoppingListItemId(itemId)),
                ModelFixture.GetShoppingListItemWithId(new ShoppingListItemId(CommonFixture.NextInt()))
            }.AsEnumerable();

            var fixure = CommonFixture.GetNewFixture();
            fixure.Inject(itemsAlreadyOnList);
            var shoppingList = fixure.Create<DomainModels.ShoppingList>();
            var collidingItem = ModelFixture.GetStoreItemWithId(new StoreItemId(itemId));

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