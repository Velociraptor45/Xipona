using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using DomainModels = ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Models.ShoppingLists
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
            Action action = () => shoppingList.AddItem(null);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void AddItem_WithItemIdIsAlreadyOnList_ShouldThrowDomainException()
        {
            // Arrange
            var shoppingList = shoppingListFixture.GetShoppingList();
            int collidingItemIndex = commonFixture.NextInt(0, shoppingList.Items.Count);
            int collidingItemId = shoppingList.Items.ElementAt(collidingItemIndex).Id.Actual.Value;

            var collidingItem = shoppingListItemFixture.GetShoppingListItemWithId(
                new ShoppingListItemId(collidingItemId));

            // Act
            Action action = () => shoppingList.AddItem(collidingItem);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ItemAlreadyOnShoppingList);
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
            var listItem = shoppingListItemFixture.GetShoppingListItemWithId(new ShoppingListItemId(storeItemId));

            // Act
            list.AddItem(listItem);

            // Assert
            using (new AssertionScope())
            {
                list.Items.Should().HaveCount(4);
                list.Items.Select(i => i.Id.Actual?.Value).Should().Contain(listItem.Id.Actual.Value);
            }
        }

        [Fact]
        public void AddItem_WithItemWithOfflineId_ShouldThrowDomainException()
        {
            // Arrange
            var list = shoppingListFixture.GetShoppingList();
            var listItem = shoppingListItemFixture.GetShoppingListItemWithId(new ShoppingListItemId(Guid.NewGuid()));

            // Act
            Action action = () => list.AddItem(listItem);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ActualIdRequired);
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
        public void RemoveItem_WithOfflineId_ShouldThrowDomainException()
        {
            // Arrange
            var list = shoppingListFixture.GetShoppingList();

            // Act
            Action action = () => list.RemoveItem(new ShoppingListItemId(Guid.NewGuid()));

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ActualIdRequired);
            }
        }

        [Fact]
        public void RemoveItem_WithShoppingListItemIdNotOnList_ShouldThrowDomainException()
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
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ItemNotOnShoppingList);
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
        public void PutItemInBasket_WithOfflineId_ShouldThrowDomainException()
        {
            // Arrange
            var list = shoppingListFixture.GetShoppingList();

            // Act
            Action action = () => list.PutItemInBasket(new ShoppingListItemId(Guid.NewGuid()));

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ActualIdRequired);
            }
        }

        [Fact]
        public void PutItemInBasket_WithShoppingListItemIdNotOnList_ShouldThrowDomainException()
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
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ItemNotOnShoppingList);
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
        public void RemoveFromBasket_WithOfflineId_ShouldThrowDomainException()
        {
            // Arrange
            var list = shoppingListFixture.GetShoppingList();

            // Act
            Action action = () => list.RemoveFromBasket(new ShoppingListItemId(Guid.NewGuid()));

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ActualIdRequired);
            }
        }

        [Fact]
        public void RemoveFromBasket_WithShoppingListItemIdNotOnList_ShouldThrowDomainException()
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
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ItemNotOnShoppingList);
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
        public void ChangeItemQuantity_WithOfflineId_ShouldThrowDomainException()
        {
            // Arrange
            var list = shoppingListFixture.GetShoppingList();

            // Act
            Action action = () => list.ChangeItemQuantity(
                new ShoppingListItemId(Guid.NewGuid()), commonFixture.NextFloat());

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ActualIdRequired);
            }
        }

        [Fact]
        public void ChangeItemQuantity_WithShoppingListItemIdNotOnList_ShouldThrowDomainException()
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
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ItemNotOnShoppingList);
            }
        }

        [Fact]
        public void ChangeItemQuantity_WithInvalidQuantity_ShouldThrowDomainException()
        {
            // Arrange
            var shoppingListItem = shoppingListItemFixture.GetShoppingListItemWithId();
            var list = shoppingListFixture.GetShoppingList(itemCount: 2, shoppingListItem.ToMonoList());
            var shoppingListItemId = new ShoppingListItemId(commonFixture.NextInt());

            // Act
            Action action = () => list.ChangeItemQuantity(shoppingListItemId, -commonFixture.NextFloat());

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.InvalidItemQuantity);
            }
        }

        [Fact]
        public void ChangeItemQuantity_WithValidItem_ShouldRemoveItemFromList()
        {
            // Arrange
            var shoppingListItem = shoppingListItemFixture.GetShoppingListItemWithId();
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