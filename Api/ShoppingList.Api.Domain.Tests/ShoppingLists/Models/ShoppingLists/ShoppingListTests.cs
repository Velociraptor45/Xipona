﻿using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using System;
using System.Linq;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Models.ShoppingLists
{
    public class ShoppingListTests
    {
        private readonly CommonFixture commonFixture;
        private readonly ShoppingListSectionMockFixture shoppingListSectionMockFixture;

        public ShoppingListTests()
        {
            commonFixture = new CommonFixture();
            shoppingListSectionMockFixture = new ShoppingListSectionMockFixture();
        }

        #region AddItem

        [Fact]
        public void AddItem_WithSectionIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var shoppingList = ShoppingListMother.ThreeSections().Create();
            var item = new ShoppingListItemBuilder().Create();

            // Act
            Action action = () => shoppingList.AddItem(item, null);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void AddItem_WithStoreItemIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var shoppingList = ShoppingListMother.ThreeSections().Create();
            SectionId sectionId = new SectionId(commonFixture.NextInt());

            // Act
            Action action = () => shoppingList.AddItem(null, sectionId);

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
            var shoppingList = ShoppingListMother.ThreeSections().Create();
            var collidingItemId = commonFixture.ChooseRandom(shoppingList.Items).Id;

            var collidingItem = new ShoppingListItemBuilder().WithId(collidingItemId).Create();
            SectionId sectionId = new SectionId(commonFixture.NextInt());

            // Act
            Action action = () => shoppingList.AddItem(collidingItem, sectionId);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ItemAlreadyOnShoppingList);
            }
        }

        [Fact]
        public void AddItem_WithSectionNotFound_ShouldThrowDomainException()
        {
            IShoppingList shoppingList = ShoppingListMother.ThreeSections().Create();
            IShoppingListItem item = new ShoppingListItemBuilder().Create();
            var existingSectionIds = shoppingList.Sections.Select(s => s.Id.Value);
            int sectionId = commonFixture.NextInt(exclude: existingSectionIds);

            // Act
            Action action = () => shoppingList.AddItem(item, new SectionId(sectionId));

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.SectionInStoreNotFound);
            }
        }

        [Fact]
        public void AddItem_WithValidItemWithActualId_ShouldAddItemToList()
        {
            // Arrange
            var sectionMocks = shoppingListSectionMockFixture.CreateMany(3).ToList();
            var shoppingList = ShoppingListMother.Initial()
                .WithSections(sectionMocks.Select(s => s.Object))
                .Create();

            ShoppingListSectionMock chosenSection = commonFixture.ChooseRandom(sectionMocks);
            IShoppingListItem item = new ShoppingListItemBuilder().Create();

            // Act
            shoppingList.AddItem(item, chosenSection.Object.Id);

            // Assert
            using (new AssertionScope())
            {
                chosenSection.VerifyAddItemOnce(item);
            }
        }

        #endregion AddItem

        #region RemoveItem

        [Fact]
        public void RemoveItem_WithItemIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var list = ShoppingListMother.ThreeSections().Create();

            // Act
            Action action = () => list.RemoveItem(null);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void RemoveItem_WithItemIdNotOnList_ShouldDoNothing()
        {
            // Arrange
            var sectionMocks = shoppingListSectionMockFixture.CreateMany(2).ToList();
            sectionMocks.ForEach(m => m.SetupContainsItem(false));
            var list = ShoppingListMother.Initial()
                .WithSections(sectionMocks.Select(s => s.Object))
                .Create();

            var itemIdsToExclude = list.Items.Select(i => i.Id.Value);
            var shoppingListItemId = new ItemId(commonFixture.NextInt(itemIdsToExclude));

            // Act
            list.RemoveItem(shoppingListItemId);

            // Assert
            using (new AssertionScope())
            {
                foreach (var mock in sectionMocks)
                {
                    mock.VerifyRemoveItemNever();
                }
            }
        }

        [Fact]
        public void RemoveItem_WithValidItemId_ShouldRemoveItemFromList()
        {
            // Arrange
            var sectionMocks = shoppingListSectionMockFixture.CreateMany(3).ToList();
            var shoppingList = ShoppingListMother.Initial()
                .WithSections(sectionMocks.Select(s => s.Object))
                .Create();

            ShoppingListSectionMock chosenSectionMock = commonFixture.ChooseRandom(sectionMocks);
            IShoppingListItem chosenItem = commonFixture.ChooseRandom(chosenSectionMock.Object.Items);

            sectionMocks.ForEach(m => m.SetupContainsItem(m == chosenSectionMock));

            // Act
            shoppingList.RemoveItem(chosenItem.Id);

            // Assert
            using (new AssertionScope())
            {
                foreach (var section in sectionMocks)
                {
                    if (section == chosenSectionMock)
                        section.VerifyRemoveItemOnce(chosenItem.Id);
                    else
                        section.VerifyRemoveItemNever();
                }
            }
        }

        #endregion RemoveItem

        #region PutItemInBasket

        [Fact]
        public void PutItemInBasket_WithItemIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var list = ShoppingListMother.ThreeSections().Create();

            // Act
            Action action = () => list.PutItemInBasket(null);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void PutItemInBasket_WithItemIdNotOnList_ShouldThrowDomainException()
        {
            // Arrange
            var list = ShoppingListMother.ThreeSections().Create();
            var itemIdsToExclude = list.Items.Select(i => i.Id.Value);
            var shoppingListItemId = new ItemId(commonFixture.NextInt(itemIdsToExclude));

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
        public void PutItemInBasket_WithValidItemId_ShouldPutItemInBasket()
        {
            // Arrange
            var sectionMocks = shoppingListSectionMockFixture.CreateMany(3).ToList();
            var shoppingList = ShoppingListMother.Initial()
                .WithSections(sectionMocks.Select(s => s.Object))
                .Create();

            ShoppingListSectionMock chosenSectionMock = commonFixture.ChooseRandom(sectionMocks);
            IShoppingListItem chosenItem = commonFixture.ChooseRandom(chosenSectionMock.Object.Items);

            sectionMocks.ForEach(m => m.SetupContainsItem(m == chosenSectionMock));

            // Act
            shoppingList.PutItemInBasket(chosenItem.Id);

            // Assert
            using (new AssertionScope())
            {
                foreach (var section in sectionMocks)
                {
                    if (section == chosenSectionMock)
                        section.VerifyPutItemInBasketOnce(chosenItem.Id);
                    else
                        section.VerifyPutItemInBasketNever();
                }
            }
        }

        #endregion PutItemInBasket

        #region RemoveFromBasket

        [Fact]
        public void RemoveFromBasket_WithItemIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var list = ShoppingListMother.ThreeSections().Create();

            // Act
            Action action = () => list.RemoveFromBasket(null);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void RemoveFromBasket_WithItemIdNotOnList_ShouldThrowDomainException()
        {
            // Arrange
            var list = ShoppingListMother.ThreeSections().Create();
            var itemIdsToExclude = list.Items.Select(i => i.Id.Value);
            var shoppingListItemId = new ItemId(commonFixture.NextInt(itemIdsToExclude));

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
        public void RemoveFromBasket_WithValidItemId_ShouldRemoveItemFromList()
        {
            // Arrange
            var sectionMocks = shoppingListSectionMockFixture.CreateMany(3).ToList();
            var shoppingList = ShoppingListMother.Initial()
                .WithSections(sectionMocks.Select(s => s.Object))
                .Create();

            ShoppingListSectionMock chosenSectionMock = commonFixture.ChooseRandom(sectionMocks);
            IShoppingListItem chosenItem = commonFixture.ChooseRandom(chosenSectionMock.Object.Items);

            sectionMocks.ForEach(m => m.SetupContainsItem(m == chosenSectionMock));

            // Act
            shoppingList.RemoveFromBasket(chosenItem.Id);

            // Assert
            using (new AssertionScope())
            {
                foreach (var section in sectionMocks)
                {
                    if (section == chosenSectionMock)
                        section.VerifyRemoveItemFromBasketOnce(chosenItem.Id);
                    else
                        section.VerifyRemoveItemFromBasketNever();
                }
            }
        }

        #endregion RemoveFromBasket

        #region ChangeItemQuantity

        [Fact]
        public void ChangeItemQuantity_WithItemIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var list = ShoppingListMother.ThreeSections().Create();

            // Act
            Action action = () => list.ChangeItemQuantity(null, commonFixture.NextFloat());

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void ChangeItemQuantity_WithItemIdNotOnList_ShouldThrowDomainException()
        {
            // Arrange
            var list = ShoppingListMother.ThreeSections().Create();
            var itemIdsToExclude = list.Items.Select(i => i.Id.Value);
            var shoppingListItemId = new ItemId(commonFixture.NextInt(itemIdsToExclude));

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
            var shoppinglist = ShoppingListMother.ThreeSections().Create();
            var chosenShoppingListItem = commonFixture.ChooseRandom(shoppinglist.Items);

            // Act
            Action action = () => shoppinglist.ChangeItemQuantity(chosenShoppingListItem.Id, -commonFixture.NextFloat());

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.InvalidItemQuantity);
            }
        }

        [Fact]
        public void ChangeItemQuantity_WithValidItemId_ShouldChangeQuantity()
        {
            // Arrange
            var sectionMocks = shoppingListSectionMockFixture.CreateMany(3).ToList();
            var shoppingList = ShoppingListMother.Initial()
                .WithSections(sectionMocks.Select(s => s.Object))
                .Create();

            ShoppingListSectionMock chosenSectionMock = commonFixture.ChooseRandom(sectionMocks);
            IShoppingListItem chosenItem = commonFixture.ChooseRandom(chosenSectionMock.Object.Items);

            sectionMocks.ForEach(m => m.SetupContainsItem(m == chosenSectionMock));

            float quantity = commonFixture.NextFloat();

            // Act
            shoppingList.ChangeItemQuantity(chosenItem.Id, quantity);

            // Assert
            using (new AssertionScope())
            {
                foreach (var section in sectionMocks)
                {
                    if (section == chosenSectionMock)
                        section.VerifyChangeItemQuantityOnce(chosenItem.Id, quantity);
                    else
                        section.VerifyChangeItemQuantityNever();
                }
            }
        }

        #endregion ChangeItemQuantity

        #region Finish

        [Fact]
        public void Finish_WithCompletedShoppingList_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var shoppingList = ShoppingListMother.Completed().Create();

            DateTime completionDate = fixture.Create<DateTime>();

            // Act
            Action action = () => shoppingList.Finish(completionDate);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ShoppingListAlreadyFinished);
            }
        }

        [Fact]
        public void Finish_WithUncompletedShoppingList_ShouldSetCompletionDate()
        {
            // Arrange
            var shoppingList = ShoppingListMother.OneSectionWithOneItemInBasketAndOneNot().Create();
            var itemsInBasket = shoppingList.Items.Where(i => i.IsInBasket);
            var itemsNotInBasket = shoppingList.Items.Where(i => !i.IsInBasket);

            DateTime completionDate = commonFixture.GetNewFixture().Create<DateTime>();

            // Act
            IShoppingList result = shoppingList.Finish(completionDate);

            // Assert
            using (new AssertionScope())
            {
                shoppingList.Sections.First().Items.Should().BeEquivalentTo(itemsInBasket);
                result.Sections.First().Items.Should().BeEquivalentTo(itemsNotInBasket);
                shoppingList.CompletionDate.Should().Be(completionDate);
            }
        }

        #endregion Finish

        #region AddSection

        [Fact]
        public void AddSection_WithSectionIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var shoppingList = ShoppingListMother.OneSectionWithOneItemInBasket().Create();

            // Act
            Action action = () => shoppingList.AddSection(null);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void AddSection_WithSectionAlreadyInShoppingList_ShouldThrowDomainException()
        {
            IShoppingList shoppingList = ShoppingListMother.OneSectionWithOneItemInBasket().Create();
            IShoppingListSection section = commonFixture.ChooseRandom(shoppingList.Sections);

            // Act
            Action action = () => shoppingList.AddSection(section);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.SectionAlreadyInShoppingList);
            }
        }

        [Fact]
        public void AddSection_WithNewSection_ShouldThrowDomainException()
        {
            IShoppingList shoppingList = ShoppingListMother.OneSectionWithOneItemInBasket().Create();
            var existingSectionIds = shoppingList.Sections.Select(s => s.Id.Value);

            var sectionId = new SectionId(commonFixture.NextInt(existingSectionIds));
            IShoppingListSection section = new ShoppingListSectionBuilder().WithId(sectionId).Create();

            // Act
            shoppingList.AddSection(section);

            // Assert
            using (new AssertionScope())
            {
                shoppingList.Sections.Should().Contain(section);
            }
        }

        #endregion AddSection
    }
}