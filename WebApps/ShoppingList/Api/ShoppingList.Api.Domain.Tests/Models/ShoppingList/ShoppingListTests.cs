using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Models.Fixtures;
using ShoppingList.Api.Core.Extensions;
using ShoppingList.Api.Core.Tests;
using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using DomainModels = ShoppingList.Api.Domain.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Models.ShoppingList
{
    public class ShoppingListTests
    {
        private readonly CommonFixture commonFixture;
        private readonly ShoppingListItemFixture shoppingListItemFixture;
        private readonly ShoppingListFixture shoppingListFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemFixture storeItemFixture;

        public ShoppingListTests()
        {
            commonFixture = new CommonFixture();
            shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
            shoppingListFixture = new ShoppingListFixture(shoppingListItemFixture, commonFixture);
            storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(storeItemAvailabilityFixture, commonFixture);
        }

        #region AddItem

        [Fact]
        public void AddItem_WithStoreItemIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixure = commonFixture.GetNewFixture();
            var shoppingList = fixure.Create<DomainModels.ShoppingList>();

            // Act
            Action action = () => shoppingList.AddItem(null, commonFixture.NextBool(), commonFixture.NextFloat());

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
            var shoppingList = shoppingListFixture.GetShoppingList();
            int collidingItemIndex = commonFixture.NextInt(0, shoppingList.Items.Count);
            int collidingItemId = shoppingList.Items.ElementAt(collidingItemIndex).Id.Actual.Value;

            var collidingItem = storeItemFixture.GetStoreItem(new StoreItemId(collidingItemId));

            // Act
            Action action = () => shoppingList.AddItem(collidingItem, commonFixture.NextBool(), commonFixture.NextFloat());

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
            var availabilities = storeItemAvailabilityFixture.GetAvailabilities(count: 3);
            var usedAvailabilitiyStores = availabilities.Select(av => av.StoreId.Value);
            var storeIdForShoppingList = commonFixture.NextInt(exclude: usedAvailabilitiyStores);

            var list = shoppingListFixture.GetShoppingList(new StoreId(storeIdForShoppingList));

            // this prevents that the item is already on the list
            var usedItemIds = list.Items.Select(i => i.Id.Actual.Value);
            int storeItemId = commonFixture.NextInt(usedItemIds);
            var storeItem = storeItemFixture.GetStoreItem(new StoreItemId(storeItemId), 0, availabilities);

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
            var availabilities = storeItemAvailabilityFixture.GetAvailabilities(count: 3).ToList();
            var storeIdForShoppingList = availabilities[commonFixture.NextInt(0, availabilities.Count - 1)].StoreId.Value;
            var list = shoppingListFixture.GetShoppingList(new StoreId(storeIdForShoppingList));

            // this prevents that the item is already on the list
            var usedItemIds = list.Items.Select(i => i.Id.Actual.Value);
            int storeItemId = commonFixture.NextInt(usedItemIds);
            var storeItem = storeItemFixture.GetStoreItem(new StoreItemId(storeItemId), 0, availabilities);

            // Act
            list.AddItem(storeItem, commonFixture.NextBool(), commonFixture.NextFloat());

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
            var list = shoppingListFixture.GetShoppingList();
            var storeItem = storeItemFixture.GetStoreItem(new StoreItemId(Guid.NewGuid()));

            // Act
            Action action = () => list.AddItem(storeItem, commonFixture.NextBool(), commonFixture.NextFloat());

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ActualIdRequiredException>();
            }
        }

        #endregion AddItem

        #region RemoveItem

        [Fact]
        public void RemoveItem_WithShoppingListItemIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var list = shoppingListFixture.GetShoppingList();

            // Act
            Action action = () => list.RemoveItem(null);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void RemoveItem_WithOfflineId_ShouldThrowActualIdRequiredException()
        {
            // Arrange
            var list = shoppingListFixture.GetShoppingList();

            // Act
            Action action = () => list.RemoveItem(new ShoppingListItemId(Guid.NewGuid()));

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ActualIdRequiredException>();
            }
        }

        [Fact]
        public void RemoveItem_WithShoppingListItemIdNotOnList_ShouldThrowItemNotOnShoppingListException()
        {
            // Arrange
            var list = shoppingListFixture.GetShoppingList();
            var itemIdsToExclude = list.Items.Select(i => i.Id.Actual.Value);
            var shoppingListItemId = new ShoppingListItemId(commonFixture.NextInt(itemIdsToExclude));

            // Act
            Action action = () => list.RemoveItem(shoppingListItemId);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ItemNotOnShoppingListException>();
            }
        }

        [Fact]
        public void RemoveItem_WithValidItem_ShouldRemoveItemFromList()
        {
            // Arrange
            var list = shoppingListFixture.GetShoppingList();
            int idToRemove = list.Items.ElementAt(commonFixture.NextInt(0, list.Items.Count - 1)).Id.Actual.Value;

            // Act
            list.RemoveItem(new ShoppingListItemId(idToRemove));

            // Assert
            using (new AssertionScope())
            {
                list.Items.Should().HaveCount(2);
                list.Items.Should().NotContain(i => i.Id == new ShoppingListItemId(idToRemove));
            }
        }

        #endregion RemoveItem

        #region PutItemInBasket

        [Fact]
        public void PutItemInBasket_WithShoppingListItemIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var list = shoppingListFixture.GetShoppingList();

            // Act
            Action action = () => list.PutItemInBasket(null);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void PutItemInBasket_WithOfflineId_ShouldThrowActualIdRequiredException()
        {
            // Arrange
            var list = shoppingListFixture.GetShoppingList();

            // Act
            Action action = () => list.PutItemInBasket(new ShoppingListItemId(Guid.NewGuid()));

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ActualIdRequiredException>();
            }
        }

        [Fact]
        public void PutItemInBasket_WithShoppingListItemIdNotOnList_ShouldThrowItemNotOnShoppingListException()
        {
            // Arrange
            var list = shoppingListFixture.GetShoppingList();
            var itemIdsToExclude = list.Items.Select(i => i.Id.Actual.Value);
            var shoppingListItemId = new ShoppingListItemId(commonFixture.NextInt(itemIdsToExclude));

            // Act
            Action action = () => list.PutItemInBasket(shoppingListItemId);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ItemNotOnShoppingListException>();
            }
        }

        [Fact]
        public void PutItemInBasket_WithValidItem_ShouldRemoveItemFromList()
        {
            // Arrange
            var shoppingListItem = shoppingListItemFixture.GetShoppingListItem(isInBasket: false);
            var list = shoppingListFixture.GetShoppingList(itemCount: 2, shoppingListItem.ToMonoList());

            // Act
            list.PutItemInBasket(shoppingListItem.Id);

            // Assert
            using (new AssertionScope())
            {
                list.Items.Should().HaveCount(3);
                list.Items.Should().Contain(i => i.Id == shoppingListItem.Id);
                list.Items.Single(i => i.Id == shoppingListItem.Id).IsInBasket.Should().BeTrue();
            }
        }

        #endregion PutItemInBasket

        #region RemoveFromBasket

        [Fact]
        public void RemoveFromBasket_WithShoppingListItemIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var list = shoppingListFixture.GetShoppingList();

            // Act
            Action action = () => list.RemoveFromBasket(null);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void RemoveFromBasket_WithOfflineId_ShouldThrowActualIdRequiredException()
        {
            // Arrange
            var list = shoppingListFixture.GetShoppingList();

            // Act
            Action action = () => list.RemoveFromBasket(new ShoppingListItemId(Guid.NewGuid()));

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ActualIdRequiredException>();
            }
        }

        [Fact]
        public void RemoveFromBasket_WithShoppingListItemIdNotOnList_ShouldThrowItemNotOnShoppingListException()
        {
            // Arrange
            var list = shoppingListFixture.GetShoppingList();
            var itemIdsToExclude = list.Items.Select(i => i.Id.Actual.Value);
            var shoppingListItemId = new ShoppingListItemId(commonFixture.NextInt(itemIdsToExclude));

            // Act
            Action action = () => list.RemoveFromBasket(shoppingListItemId);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ItemNotOnShoppingListException>();
            }
        }

        [Fact]
        public void RemoveFromBasket_WithValidItem_ShouldRemoveItemFromList()
        {
            // Arrange
            var shoppingListItem = shoppingListItemFixture.GetShoppingListItem(isInBasket: true);
            var list = shoppingListFixture.GetShoppingList(itemCount: 2, shoppingListItem.ToMonoList());

            // Act
            list.RemoveFromBasket(shoppingListItem.Id);

            // Assert
            using (new AssertionScope())
            {
                list.Items.Should().HaveCount(3);
                list.Items.Should().Contain(i => i.Id == shoppingListItem.Id);
                list.Items.Single(i => i.Id == shoppingListItem.Id).IsInBasket.Should().BeFalse();
            }
        }

        #endregion RemoveFromBasket

        #region ChangeItemQuantity

        [Fact]
        public void ChangeItemQuantity_WithShoppingListItemIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var list = shoppingListFixture.GetShoppingList();

            // Act
            Action action = () => list.ChangeItemQuantity(null, commonFixture.NextFloat());

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void ChangeItemQuantity_WithOfflineId_ShouldThrowActualIdRequiredException()
        {
            // Arrange
            var list = shoppingListFixture.GetShoppingList();

            // Act
            Action action = () => list.ChangeItemQuantity(
                new ShoppingListItemId(Guid.NewGuid()), commonFixture.NextFloat());

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ActualIdRequiredException>();
            }
        }

        [Fact]
        public void ChangeItemQuantity_WithShoppingListItemIdNotOnList_ShouldThrowItemNotOnShoppingListException()
        {
            // Arrange
            var list = shoppingListFixture.GetShoppingList();
            var itemIdsToExclude = list.Items.Select(i => i.Id.Actual.Value);
            var shoppingListItemId = new ShoppingListItemId(commonFixture.NextInt(itemIdsToExclude));

            // Act
            Action action = () => list.ChangeItemQuantity(shoppingListItemId, commonFixture.NextFloat());

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ItemNotOnShoppingListException>();
            }
        }

        [Fact]
        public void ChangeItemQuantity_WithInvalidQuantity_ShouldThrowInvalidItemQuantityException()
        {
            // Arrange
            var shoppingListItem = shoppingListItemFixture.GetShoppingListItem();
            var list = shoppingListFixture.GetShoppingList(itemCount: 2, shoppingListItem.ToMonoList());
            var shoppingListItemId = new ShoppingListItemId(commonFixture.NextInt());

            // Act
            Action action = () => list.ChangeItemQuantity(shoppingListItemId, -commonFixture.NextFloat());

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<InvalidItemQuantityException>();
            }
        }

        [Fact]
        public void ChangeItemQuantity_WithValidItem_ShouldRemoveItemFromList()
        {
            // Arrange
            var shoppingListItem = shoppingListItemFixture.GetShoppingListItem();
            var list = shoppingListFixture.GetShoppingList(itemCount: 2, shoppingListItem.ToMonoList());
            var expectedQuantity = commonFixture.NextFloat();

            // Act
            list.ChangeItemQuantity(shoppingListItem.Id, expectedQuantity);

            // Assert
            using (new AssertionScope())
            {
                list.Items.Should().HaveCount(3);
                list.Items.Should().Contain(i => i.Id == shoppingListItem.Id);
                list.Items.Single(i => i.Id == shoppingListItem.Id).Quantity.Should().Be(expectedQuantity);
            }
        }

        #endregion ChangeItemQuantity

        #region Finish

        [Theory]
        [ClassData(typeof(FinishShoppingListTestData))]
        public void Finish__ShouldCreateNewListWithItemsNotInBasketAndRemoveThemFromCurrentList(
            List<ShoppingListItem> itemsNotInBasket, List<ShoppingListItem> itemsInBasket)
        {
            // Arrange
            var allItems = new List<ShoppingListItem>(itemsInBasket);
            allItems.AddRange(itemsNotInBasket);
            allItems.Shuffle();

            DateTime completionDate = commonFixture.NextDate();
            var list = shoppingListFixture.GetShoppingList(itemCount: 0, additionalItems: allItems);

            // Act
            var newList = list.Finish(completionDate);

            // Assert
            using (new AssertionScope())
            {
                list.Items.Should().HaveCount(itemsInBasket.Count);
                list.Items.Should().BeEquivalentTo(itemsInBasket);
                newList.Items.Should().HaveCount(itemsNotInBasket.Count);
                newList.Items.Should().BeEquivalentTo(itemsNotInBasket);
                list.CompletionDate.Should().Be(completionDate);
                newList.CompletionDate.Should().BeNull();
            }
        }

        #endregion Finish
    }
}