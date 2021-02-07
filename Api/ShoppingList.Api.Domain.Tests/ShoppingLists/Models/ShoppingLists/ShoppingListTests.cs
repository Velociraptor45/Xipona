using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Mocks;
using System;
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
        private readonly ShoppingListFixture shoppingListFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemFixture storeItemFixture;
        private readonly ShoppingListSectionMockFixture shoppingListSectionMockFixture;

        public ShoppingListTests()
        {
            commonFixture = new CommonFixture();
            shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
            shoppingListSectionFixture = new ShoppingListSectionFixture(commonFixture, shoppingListItemFixture);
            shoppingListFixture = new ShoppingListFixture(shoppingListSectionFixture, commonFixture);
            storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(storeItemAvailabilityFixture, commonFixture);
            shoppingListSectionMockFixture = new ShoppingListSectionMockFixture(shoppingListSectionFixture, commonFixture);
        }

        //todo: check everything -,-

        #region AddItem

        [Fact]
        public void AddItem_WithStoreItemIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixure = commonFixture.GetNewFixture();
            var shoppingList = fixure.Create<DomainModels.ShoppingList>();
            var sectionId = commonFixture.GetNewFixture().Create<ShoppingListSectionId>();

            // Act
            Action action = () => shoppingList.AddItem(null, sectionId);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void AddItem_WithItemWithOfflineId_ShouldThrowDomainException()
        {
            // Arrange
            var list = shoppingListFixture.Create();
            var listItem = shoppingListItemFixture.GetShoppingListItemWithId(new ShoppingListItemId(Guid.NewGuid()));
            var sectionId = commonFixture.GetNewFixture().Create<ShoppingListSectionId>();

            // Act
            Action action = () => list.AddItem(listItem, sectionId);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ActualIdRequired);
            }
        }

        [Fact]
        public void AddItem_WithItemIdIsAlreadyOnList_ShouldThrowDomainException()
        {
            // Arrange
            var shoppingList = shoppingListFixture.Create();
            int collidingItemIndex = commonFixture.NextInt(0, shoppingList.Items.Count);
            int collidingItemId = shoppingList.Items.ElementAt(collidingItemIndex).Id.Actual.Value;

            var collidingItem = shoppingListItemFixture.GetShoppingListItemWithId(
                new ShoppingListItemId(collidingItemId));
            var sectionId = commonFixture.GetNewFixture().Create<ShoppingListSectionId>();

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
        public void AddItem_WithNoDefaultSection_ShouldThrowDomainException()
        {
            // Arrange
            var listDefinition = new ShoppingListGenerationDefinition()
            {
                SectionDefinitions = new ShoppingListSectionGenerationDefinition() { IsDefaultSection = false }.ToMonoList()
            };
            IShoppingList shoppingList = shoppingListFixture.Create(listDefinition);
            IShoppingListItem item = shoppingListItemFixture.CreateUnique(shoppingList);

            // Act
            Action action = () => shoppingList.AddItem(item, null);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.NoDefaultSectionSpecified);
            }
        }

        [Fact]
        public void AddItem_WithSectionNotFound_ShouldThrowDomainException()
        {
            IShoppingList shoppingList = shoppingListFixture.Create();
            IShoppingListItem item = shoppingListItemFixture.CreateUnique(shoppingList);
            var existingSectionIds = shoppingList.Sections.Select(s => s.Id.Value);
            int sectionId = commonFixture.NextInt(exclude: existingSectionIds);

            // Act
            Action action = () => shoppingList.AddItem(item, new ShoppingListSectionId(sectionId));

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<DomainException>()
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.SectionNotPartOfStore);
            }
        }

        [Fact]
        public void AddItem_WithValidItemWithActualId_ShouldAddItemToList()
        {
            // Arrange
            var sections = shoppingListSectionMockFixture.CreateMany(3).ToList();

            var listDefinition = new ShoppingListGenerationDefinition()
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
        public void RemoveItem_WithShoppingListItemIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var list = shoppingListFixture.Create();

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
            var list = shoppingListFixture.Create();

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
            var list = shoppingListFixture.Create();
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
            var sections = shoppingListSectionMockFixture.CreateMany(3).ToList();

            var listDefinition = new ShoppingListGenerationDefinition()
            {
                Sections = sections.Select(s => s.Object)
            };
            var shoppingList = shoppingListFixture.Create(listDefinition);

            ShoppingListSectionMock chosenSection = commonFixture.ChooseRandom(sections);
            IShoppingListItem chosenItem = commonFixture.ChooseRandom(chosenSection.Object.ShoppingListItems);

            chosenSection.SetupContainsItem(chosenItem.Id, true);

            // Act
            shoppingList.RemoveItem(chosenItem.Id);

            // Assert
            using (new AssertionScope())
            {
                foreach (var section in sections)
                {
                    if (section == chosenSection)
                        section.VerifyRemoveItemOnce(chosenItem.Id);
                    else
                        section.VerifyRemoveItemNever();
                }
            }
        }

        #endregion RemoveItem

        #region PutItemInBasket

        [Fact]
        public void PutItemInBasket_WithShoppingListItemIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var list = shoppingListFixture.Create();

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
            var list = shoppingListFixture.Create();

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
            var list = shoppingListFixture.Create();
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
        public void PutItemInBasket_WithValidItem_ShouldPutItemInBasket()
        {
            // Arrange
            var sections = shoppingListSectionMockFixture.CreateMany(3).ToList();

            var listDefinition = new ShoppingListGenerationDefinition()
            {
                Sections = sections.Select(s => s.Object)
            };
            var shoppingList = shoppingListFixture.Create(listDefinition);

            ShoppingListSectionMock chosenSection = commonFixture.ChooseRandom(sections);
            IShoppingListItem chosenItem = commonFixture.ChooseRandom(chosenSection.Object.ShoppingListItems);
            chosenSection.SetupContainsItem(chosenItem.Id, true);

            // Act
            shoppingList.PutItemInBasket(chosenItem.Id);

            // Assert
            using (new AssertionScope())
            {
                foreach (var section in sections)
                {
                    if (section == chosenSection)
                        section.VerifyPutItemInBasketOnce(chosenItem.Id);
                    else
                        section.VerifyPutItemInBasketNever();
                }
            }
        }

        #endregion PutItemInBasket

        #region RemoveFromBasket

        [Fact]
        public void RemoveFromBasket_WithShoppingListItemIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var list = shoppingListFixture.Create();

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
            var list = shoppingListFixture.Create();

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
            var list = shoppingListFixture.Create();
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
            var sections = shoppingListSectionMockFixture.CreateMany(3).ToList();

            var listDefinition = new ShoppingListGenerationDefinition()
            {
                Sections = sections.Select(s => s.Object)
            };
            var shoppingList = shoppingListFixture.Create(listDefinition);

            ShoppingListSectionMock chosenSection = commonFixture.ChooseRandom(sections);
            IShoppingListItem chosenItem = commonFixture.ChooseRandom(chosenSection.Object.ShoppingListItems);
            chosenSection.SetupContainsItem(chosenItem.Id, true);

            // Act
            shoppingList.RemoveFromBasket(chosenItem.Id);

            // Assert
            using (new AssertionScope())
            {
                foreach (var section in sections)
                {
                    if (section == chosenSection)
                        section.VerifyRemoveItemFromBasketOnce(chosenItem.Id);
                    else
                        section.VerifyRemoveItemFromBasketNever();
                }
            }
        }

        #endregion RemoveFromBasket

        #region ChangeItemQuantity

        [Fact]
        public void ChangeItemQuantity_WithShoppingListItemIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var list = shoppingListFixture.Create();

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
            var list = shoppingListFixture.Create();

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
            var list = shoppingListFixture.Create();
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
            var shoppinglist = shoppingListFixture.Create();
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
        public void ChangeItemQuantity_WithValidItem_ShouldChangeQuantity()
        {
            // Arrange
            var sections = shoppingListSectionMockFixture.CreateMany(3).ToList();

            var listDefinition = new ShoppingListGenerationDefinition()
            {
                Sections = sections.Select(s => s.Object)
            };
            var shoppingList = shoppingListFixture.Create(listDefinition);

            ShoppingListSectionMock chosenSection = commonFixture.ChooseRandom(sections);
            IShoppingListItem chosenItem = commonFixture.ChooseRandom(chosenSection.Object.ShoppingListItems);
            chosenSection.SetupContainsItem(chosenItem.Id, true);

            float quantity = commonFixture.NextFloat();

            // Act
            shoppingList.ChangeItemQuantity(chosenItem.Id, quantity);

            // Assert
            using (new AssertionScope())
            {
                foreach (var section in sections)
                {
                    if (section == chosenSection)
                        section.VerifyChangeItemQuantityOnce(chosenItem.Id, quantity);
                    else
                        section.VerifyChangeItemQuantityNever();
                }
            }
        }

        #endregion ChangeItemQuantity

        #region Finish

        // todo: implement tests for new finish logic

        #endregion Finish
    }
}