using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common.Fixtures;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Mocks;
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
        private readonly ShoppingListSectionFixture shoppingListSectionFixture;
        private readonly IModelFixture<IShoppingList, ShoppingListDefinition> shoppingListFixture;
        private readonly ShoppingListSectionMockFixture shoppingListSectionMockFixture;

        public ShoppingListTests()
        {
            commonFixture = new CommonFixture();
            shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
            shoppingListSectionFixture = new ShoppingListSectionFixture(commonFixture);
            shoppingListFixture = new ShoppingListFixture(commonFixture).AsModelFixture();
            shoppingListSectionMockFixture = new ShoppingListSectionMockFixture(shoppingListSectionFixture, commonFixture);
        }

        #region AddItem

        [Fact]
        public void AddItem_WithSectionIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixure = commonFixture.GetNewFixture();
            var shoppingList = fixure.Create<DomainModels.ShoppingList>();
            var item = shoppingListItemFixture.Create(new ItemId(commonFixture.NextInt()));

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
            var fixure = commonFixture.GetNewFixture();
            var shoppingList = fixure.Create<DomainModels.ShoppingList>();
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
            var shoppingList = shoppingListFixture.CreateValid();
            int collidingItemId = commonFixture.ChooseRandom(shoppingList.Items).Id.Value;

            var collidingItem = shoppingListItemFixture.Create(new ItemId(collidingItemId));
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
            IShoppingList shoppingList = shoppingListFixture.CreateValid();
            IShoppingListItem item = shoppingListItemFixture.CreateUnique(shoppingList);
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
            var sections = shoppingListSectionMockFixture.CreateMany(3).ToList();

            var listDefinition = new ShoppingListDefinition()
            {
                Sections = sections.Select(s => s.Object)
            };
            var shoppingList = shoppingListFixture.Create(listDefinition);

            ShoppingListSectionMock chosenSection = commonFixture.ChooseRandom(sections);
            IShoppingListItem item = shoppingListItemFixture.CreateUnique(shoppingList);

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
            var list = shoppingListFixture.CreateValid();

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

            var listDef = new ShoppingListDefinition
            {
                Sections = sectionMocks.Select(s => s.Object)
            };
            var list = shoppingListFixture.CreateValid(listDef);
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

            var listDefinition = new ShoppingListDefinition()
            {
                Sections = sectionMocks.Select(s => s.Object)
            };
            var shoppingList = shoppingListFixture.Create(listDefinition);

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
            var list = shoppingListFixture.CreateValid();

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
            var list = shoppingListFixture.CreateValid();
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

            var listDefinition = new ShoppingListDefinition()
            {
                Sections = sectionMocks.Select(s => s.Object)
            };
            var shoppingList = shoppingListFixture.Create(listDefinition);

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
            var list = shoppingListFixture.CreateValid();

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
            var list = shoppingListFixture.CreateValid();
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

            var listDefinition = new ShoppingListDefinition()
            {
                Sections = sectionMocks.Select(s => s.Object)
            };
            var shoppingList = shoppingListFixture.Create(listDefinition);

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
            var list = shoppingListFixture.CreateValid();

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
            var list = shoppingListFixture.CreateValid();
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
            var shoppinglist = shoppingListFixture.CreateValid();
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

            var listDefinition = new ShoppingListDefinition()
            {
                Sections = sectionMocks.Select(s => s.Object)
            };
            var shoppingList = shoppingListFixture.Create(listDefinition);

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

            var listDefinition = new ShoppingListDefinition() { CompletionDate = fixture.Create<DateTime>() };
            var shoppingList = shoppingListFixture.Create(listDefinition);

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
            var itemInBasket = shoppingListItemFixture.CreateValidWithBasketStatus(true);
            var itemNotInBasket = shoppingListItemFixture.CreateValidWithBasketStatus(false);

            var itemInBasketSection = shoppingListSectionFixture.CreateValidWithItem(itemInBasket);
            var itemNotInBasketSection = shoppingListSectionFixture.CreateValidWithItem(itemNotInBasket);

            var listDefinition = new ShoppingListDefinition
            {
                CompletionDate = null,
                Sections = new[] { itemInBasketSection, itemNotInBasketSection }
            };
            var shoppingList = shoppingListFixture.Create(listDefinition);

            DateTime completionDate = commonFixture.GetNewFixture().Create<DateTime>();

            // Act
            IShoppingList result = shoppingList.Finish(completionDate);

            // Assert
            var expectedResult = new DomainModels.ShoppingList(
                shoppingList.Id,
                shoppingList.StoreId,
                null,
                new List<IShoppingListSection>
                {
                    new ShoppingListSection(
                        itemNotInBasketSection.Id,
                        new List<IShoppingListItem>
                        {
                            new ShoppingListItem(
                                itemNotInBasket.Id,
                                isInBasket: false,
                                itemNotInBasket.Quantity)
                        }),
                    new ShoppingListSection(
                        itemInBasketSection.Id,
                        Enumerable.Empty<IShoppingListItem>())
                });

            var expectedRemaining = new DomainModels.ShoppingList(
                shoppingList.Id,
                shoppingList.StoreId,
                completionDate,
                new List<IShoppingListSection>
                {
                    new ShoppingListSection(
                        itemInBasketSection.Id,
                        new List<IShoppingListItem>
                        {
                            new ShoppingListItem(
                                itemInBasket.Id,
                                true,
                                itemInBasket.Quantity)
                        }),
                    new ShoppingListSection(
                        itemNotInBasketSection.Id,
                        Enumerable.Empty<IShoppingListItem>())
                });

            using (new AssertionScope())
            {
                shoppingList.Should().BeEquivalentTo(expectedRemaining);
                result.Should().BeEquivalentTo(expectedResult);
            }
        }

        #endregion Finish

        #region AddSection

        [Fact]
        public void AddSection_WithSectionIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixure = commonFixture.GetNewFixture();
            var shoppingList = fixure.Create<DomainModels.ShoppingList>();

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
            IShoppingList shoppingList = shoppingListFixture.CreateValid();
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
            IShoppingList shoppingList = shoppingListFixture.CreateValid();
            var existingSectionIds = shoppingList.Sections.Select(s => s.Id.Value);

            var sectionDef = ShoppingListSectionDefinition.FromId(commonFixture.NextInt(existingSectionIds));
            IShoppingListSection section = shoppingListSectionFixture.CreateValid(sectionDef);

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