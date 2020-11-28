using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Models.Fixtures;
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
            var shoppingListFixture = new ShoppingListFixture();
            var storeItemFixture = new StoreItemFixture();

            var itemId = commonFixture.NextInt();
            var shoppingList = shoppingListFixture.GetShoppingList(2, new List<int> { itemId });
            var collidingItem = storeItemFixture.GetStoreItem(new StoreItemId(itemId));

            // Act
            Action action = () => shoppingList.AddItem(collidingItem, false, 20.4f);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ItemAlreadyOnShoppingListException>();
            }
        }

        [Fact]
        public void AddItem_WithNoAvailabilityForListStore_ShouldThrowItemAtStoreNotAvailableException()
        {
            // Arrange
            var commonFixture = new CommonFixture();
            var shoppingListFixture = new ShoppingListFixture();
            var storeItemFixture = new StoreItemFixture();

            var storeId = commonFixture.NextInt();
            var list = shoppingListFixture.GetShoppingList(3, storeId: new StoreId(storeId));

            var excludeAsItemId = list.Items.Select(i => i.Id.Actual.Value);
            var storeIdsForAvailablity = new List<int> { commonFixture.NextInt(storeId), commonFixture.NextInt(storeId) };
            var storeItem = storeItemFixture.GetStoreItem(commonFixture.NextInt(excludeAsItemId), 2, storeIdsForAvailablity);

            // Act
            Action action = () => list.AddItem(storeItem, commonFixture.NextBool(), commonFixture.NextFloat());

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ItemAtStoreNotAvailableException>();
            }
        }

        [Fact]
        public void AddItem_WithValidItemWithActualId_ShouldAddItemToList()
        {
            // Arrange
            var commonFixture = new CommonFixture();
            var shoppingListFixture = new ShoppingListFixture();
            var storeItemFixture = new StoreItemFixture();

            var storeId = commonFixture.NextInt();
            var list = shoppingListFixture.GetShoppingList(3, storeId: new StoreId(storeId));

            var excludeAsItemId = list.Items.Select(i => i.Id.Actual.Value);
            var storeItem = storeItemFixture.GetStoreItem(commonFixture.NextInt(excludeAsItemId), 2, new List<int> { storeId });
            bool isItemInBasket = commonFixture.NextBool();
            float itemQuantity = commonFixture.NextFloat();

            // Act
            list.AddItem(storeItem, isItemInBasket, itemQuantity);

            // Assert
            using (new AssertionScope())
            {
                list.Items.Should().HaveCount(4);
                list.Items.Select(i => i.Id.Actual?.Value).Should().Contain(storeItem.Id.Actual.Value);
            }
        }

        [Fact]
        public void AddItem_WithItemWithOfflineId_ShouldThrowActualIdRequiredException()
        {
            // Arrange
            var commonFixture = new CommonFixture();
            var shoppingListFixture = new ShoppingListFixture();
            var storeItemFixture = new StoreItemFixture();

            var list = shoppingListFixture.GetShoppingList(3);
            var storeItem = storeItemFixture.GetStoreItem(new StoreItemId(Guid.NewGuid()));

            // Act
            Action action = () => list.AddItem(storeItem, commonFixture.NextBool(), commonFixture.NextFloat());

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ActualIdRequiredException>();
            }
        }
    }
}