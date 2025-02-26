﻿using Force.DeepCloner;
using ProjectHermes.Xipona.Api.Core.TestKit.Services;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.Xipona.Api.Domain.TestKit.Shared;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using ProjectHermes.Xipona.Api.TestTools.Extensions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.ShoppingLists.Models.ShoppingLists;

public class ShoppingListTests
{
    public class AddItem
    {
        private readonly AddItemFixture _fixture = new();

        [Fact]
        public void AddItem_WithItemIdIsAlreadyOnList_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupSections();
            _fixture.SetupValidSectionId();
            var sut = _fixture.CreateSut();
            _fixture.SetupItemAlreadyOnShoppingList(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.SectionId);

            // Act
            var action = () => sut.AddItem(_fixture.Item, _fixture.SectionId.Value);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.ItemAlreadyOnShoppingList);
        }

        [Fact]
        public void AddItem_WithSectionNotFound_ShouldThrowDomainException()
        {
            _fixture.SetupSections();
            _fixture.SetupItem();
            _fixture.SetupInvalidSectionId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.SectionId);

            // Act
            var action = () => sut.AddItem(_fixture.Item, _fixture.SectionId.Value);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.SectionNotPartOfStore);
        }

        [Fact]
        public void AddItem_WithValidItemWithActualId_ShouldAddItemToList()
        {
            // Arrange
            _fixture.SetupSections();
            _fixture.SetupItem();
            _fixture.SetupValidSectionId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.SectionId);

            // Act
            sut.AddItem(_fixture.Item, _fixture.SectionId.Value);

            // Assert
            _fixture.VerifySectionContainsItem(sut);
        }

        private sealed class AddItemFixture : ShoppingListFixture
        {
            private List<ShoppingListSection>? _sections;
            public ShoppingListItem? Item { get; private set; }
            public SectionId? SectionId { get; private set; }

            public void SetupSections()
            {
                _sections = new ShoppingListSectionBuilder().CreateMany(3).ToList();
                Builder.WithSections(_sections);
            }

            public void SetupItem()
            {
                Item = new ShoppingListItemBuilder().Create();
            }

            public void SetupItemAlreadyOnShoppingList(ShoppingList sut)
            {
                Item = CommonFixture.ChooseRandom(sut.Items);
            }

            public void SetupValidSectionId()
            {
                TestPropertyNotSetException.ThrowIfNull(_sections);

                SectionId = CommonFixture.ChooseRandom(_sections).Id;
            }

            public void SetupInvalidSectionId()
            {
                SectionId = Domain.Stores.Models.SectionId.New;
            }

            public void VerifySectionContainsItem(ShoppingList result)
            {
                TestPropertyNotSetException.ThrowIfNull(SectionId);
                result.Sections.First(s => s.Id == SectionId.Value).Items.Should().Contain(x => x == Item);
            }
        }
    }

    public class RemoveItemAndItsTypes
    {
        private readonly RemoveItemAndItsTypesFixture _fixture = new();

        [Fact]
        public void RemoveItemAndItsTypes_WithItemIdNotOnList_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupItemId();
            var sut = _fixture.CreateSut();
            var expected = sut.DeepClone();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);

            // Act
            sut.RemoveItemAndItsTypes(_fixture.ItemId.Value);

            // Assert
            sut.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void RemoveItemAndItsTypes_WithValidItemId_WithItemWithoutTypes_ShouldRemoveItemFromList()
        {
            // Arrange
            _fixture.SetupItemId();
            _fixture.SetupItem();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);

            // Act
            sut.RemoveItemAndItsTypes(_fixture.ItemId.Value);

            // Assert
            sut.Items.Should().HaveCount(2);
            sut.Items.Should().NotContain(i => i.Id == _fixture.ItemId.Value);
        }

        [Fact]
        public void RemoveItemAndItsTypes_WithValidItemId_WithItemWithTypes_ShouldRemoveItemFromList()
        {
            // Arrange
            _fixture.SetupItemId();
            _fixture.SetupItemType();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);

            // Act
            sut.RemoveItemAndItsTypes(_fixture.ItemId.Value);

            // Assert
            sut.Items.Should().HaveCount(2);
            sut.Items.Should().NotContain(i => i.Id == _fixture.ItemId.Value);
        }

        private sealed class RemoveItemAndItsTypesFixture : ShoppingListFixture
        {
            public ItemId? ItemId { get; private set; }

            public void SetupItemId()
            {
                ItemId = Domain.Items.Models.ItemId.New;
            }

            public void SetupItem()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemId);

                var items = new List<ShoppingListItem>()
                {
                    new ShoppingListItemBuilder().WithId(ItemId.Value).WithoutTypeId().Create(),
                    new ShoppingListItemBuilder().Create(),
                    new ShoppingListItemBuilder().Create(),
                };
                items.Shuffle();
                var section = ShoppingListSectionMother.Items(items).Create();
                Builder.WithSection(section);
            }

            public void SetupItemType()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemId);

                var items = new List<ShoppingListItem>()
                {
                    new ShoppingListItemBuilder().WithId(ItemId.Value).Create(),
                    new ShoppingListItemBuilder().Create(),
                    new ShoppingListItemBuilder().Create(),
                };
                items.Shuffle();
                var section = ShoppingListSectionMother.Items(items).Create();
                Builder.WithSection(section);
            }
        }
    }

    public class RemoveItem
    {
        private readonly ShoppingListSectionMockFixture _shoppingListSectionMockFixture = new();

        [Fact]
        public void RemoveItem_WithItemIdNotOnList_ShouldDoNothing()
        {
            // Arrange
            var sectionMocks = _shoppingListSectionMockFixture.CreateMany(2).ToList();
            sectionMocks.ForEach(m => m.SetupContainsItem(false));
            var list = ShoppingListMother.Initial()
                .WithSections(sectionMocks.Select(s => s.Object))
                .Create();

            var shoppingListItemId = new ItemId(Guid.NewGuid());

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
            var sectionMocks = _shoppingListSectionMockFixture.CreateMany(3).ToList();
            var shoppingList = ShoppingListMother.Initial()
                .WithSections(sectionMocks.Select(s => s.Object))
                .Create();

            ShoppingListSectionMock chosenSectionMock = CommonFixture.ChooseRandom(sectionMocks);
            ShoppingListItem chosenItem = CommonFixture.ChooseRandom(chosenSectionMock.Object.Items);

            foreach (var sectionMock in sectionMocks)
                sectionMock.SetupContainsItem(chosenItem.Id, chosenItem.TypeId,
                    sectionMock.Object.IsEquivalentTo(chosenSectionMock.Object));

            // Act
            shoppingList.RemoveItem(chosenItem.Id, chosenItem.TypeId);

            // Assert
            using (new AssertionScope())
            {
                foreach (var section in sectionMocks)
                {
                    if (section == chosenSectionMock)
                        section.VerifyRemoveItem(chosenItem.Id, chosenItem.TypeId, Times.Once);
                    else
                        section.VerifyRemoveItemNever();
                }
            }
        }
    }

    public class PutItemInBasket
    {
        private readonly ShoppingListSectionMockFixture _shoppingListSectionMockFixture = new();

        [Fact]
        public void PutItemInBasket_WithItemIdNotOnList_ShouldThrowDomainException()
        {
            // Arrange
            var list = ShoppingListMother.ThreeSections().Create();
            var shoppingListItemId = new ItemId(Guid.NewGuid());

            // Act
            var action = () => list.PutItemInBasket(shoppingListItemId);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.ItemNotOnShoppingList);
        }

        [Fact]
        public void PutItemInBasket_WithValidItemId_ShouldPutItemInBasket()
        {
            // Arrange
            var sectionMocks = _shoppingListSectionMockFixture.CreateMany(3).ToList();
            var shoppingList = ShoppingListMother.Initial()
                .WithSections(sectionMocks.Select(s => s.Object))
                .Create();

            ShoppingListSectionMock chosenSectionMock = CommonFixture.ChooseRandom(sectionMocks);
            ShoppingListItem chosenItem = CommonFixture.ChooseRandom(chosenSectionMock.Object.Items);

            foreach (var sectionMock in sectionMocks)
                sectionMock.SetupContainsItem(chosenItem.Id, chosenItem.TypeId,
                    sectionMock.Object.IsEquivalentTo(chosenSectionMock.Object));

            // Act
            shoppingList.PutItemInBasket(chosenItem.Id, chosenItem.TypeId);

            // Assert
            using (new AssertionScope())
            {
                foreach (var section in sectionMocks)
                {
                    if (section == chosenSectionMock)
                        section.VerifyPutItemInBasketOnce(chosenItem.Id, chosenItem.TypeId);
                    else
                        section.VerifyPutItemInBasketNever();
                }
            }
        }
    }

    public class RemoveFromBasket
    {
        private readonly ShoppingListSectionMockFixture _shoppingListSectionMockFixture = new();

        [Fact]
        public void RemoveFromBasket_WithItemIdNotOnList_ShouldThrowDomainException()
        {
            // Arrange
            var list = ShoppingListMother.ThreeSections().Create();
            var shoppingListItemId = new ItemId(Guid.NewGuid());

            // Act
            Action action = () => list.RemoveFromBasket(shoppingListItemId, null);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.ItemNotOnShoppingList);
        }

        [Fact]
        public void RemoveFromBasket_WithValidItemId_ShouldRemoveItemFromList()
        {
            // Arrange
            var sectionMocks = _shoppingListSectionMockFixture.CreateMany(3).ToList();
            var shoppingList = ShoppingListMother.Initial()
                .WithSections(sectionMocks.Select(s => s.Object))
                .Create();

            ShoppingListSectionMock chosenSectionMock = CommonFixture.ChooseRandom(sectionMocks);
            ShoppingListItem chosenItem = CommonFixture.ChooseRandom(chosenSectionMock.Object.Items);

            foreach (var sectionMock in sectionMocks)
                sectionMock.SetupContainsItem(chosenItem.Id, chosenItem.TypeId,
                    sectionMock.Object.IsEquivalentTo(chosenSectionMock.Object));

            // Act
            shoppingList.RemoveFromBasket(chosenItem.Id, chosenItem.TypeId);

            // Assert
            using (new AssertionScope())
            {
                foreach (var section in sectionMocks)
                {
                    if (section == chosenSectionMock)
                        section.VerifyRemoveItemFromBasketOnce(chosenItem.Id, chosenItem.TypeId);
                    else
                        section.VerifyRemoveItemFromBasketNever();
                }
            }
        }
    }

    public class ChangeItemQuantity
    {
        private readonly ShoppingListSectionMockFixture _shoppingListSectionMockFixture = new();

        [Fact]
        public void ChangeItemQuantity_WithItemIdNotOnList_ShouldThrowDomainException()
        {
            // Arrange
            var list = ShoppingListMother.ThreeSections().Create();
            var shoppingListItemId = new ItemId(Guid.NewGuid());
            var quantity = new QuantityInBasketBuilder().Create();

            // Act
            Action action = () => list.ChangeItemQuantity(shoppingListItemId, null, quantity);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.ItemNotOnShoppingList);
        }

        [Fact]
        public void ChangeItemQuantity_WithValidItemId_ShouldChangeQuantity()
        {
            // Arrange
            var sectionMocks = _shoppingListSectionMockFixture.CreateMany(3).ToList();
            var shoppingList = ShoppingListMother.Initial()
                .WithSections(sectionMocks.Select(s => s.Object))
                .Create();

            ShoppingListSectionMock chosenSectionMock = CommonFixture.ChooseRandom(sectionMocks);
            ShoppingListItem chosenItem = CommonFixture.ChooseRandom(chosenSectionMock.Object.Items);

            foreach (var sectionMock in sectionMocks)
            {
                sectionMock.SetupContainsItem(chosenItem.Id, chosenItem.TypeId,
                    sectionMock.Object.IsEquivalentTo(chosenSectionMock.Object));
            }

            var quantity = new QuantityInBasketBuilder().Create();

            // Act
            shoppingList.ChangeItemQuantity(chosenItem.Id, chosenItem.TypeId, quantity);

            // Assert
            using (new AssertionScope())
            {
                foreach (var section in sectionMocks)
                {
                    if (section == chosenSectionMock)
                        section.VerifyChangeItemQuantityOnce(chosenItem.Id, chosenItem.TypeId, quantity);
                    else
                        section.VerifyChangeItemQuantityNever();
                }
            }
        }
    }

    public class Finish
    {
        [Fact]
        public void Finish_WithCompletedShoppingList_ShouldThrowDomainException()
        {
            // Arrange
            var shoppingList = ShoppingListMother.Completed().Create();
            var completionDate = new DomainTestBuilder<DateTimeOffset>().Create();
            var dateTimeServiceMock = new DateTimeServiceMock(MockBehavior.Strict);

            // Act
            var action = () => shoppingList.Finish(completionDate, dateTimeServiceMock.Object);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.ShoppingListAlreadyFinished);
        }

        [Fact]
        public void Finish_WithUncompletedShoppingList_ShouldSetCompletionDate()
        {
            // Arrange
            var shoppingList = ShoppingListMother.OneSectionWithOneItemInBasketAndOneNot().Create();
            var itemsInBasket = shoppingList.Items.Where(i => i.IsInBasket);
            var itemsNotInBasket = shoppingList.Items.Where(i => !i.IsInBasket);

            var completionDate = new DomainTestBuilder<DateTimeOffset>().Create();
            var dateTimeServiceMock = new DateTimeServiceMock(MockBehavior.Strict);
            var expectedCreatedAt = new DomainTestBuilder<DateTimeOffset>().Create();
            dateTimeServiceMock.SetupUtcNow(expectedCreatedAt);

            // Act
            var result = shoppingList.Finish(completionDate, dateTimeServiceMock.Object);

            // Assert
            using (new AssertionScope())
            {
                shoppingList.Sections.First().Items.Should().BeEquivalentTo(itemsInBasket);
                shoppingList.CompletionDate.Should().Be(completionDate);
                result.Sections.First().Items.Should().BeEquivalentTo(itemsNotInBasket);
                result.CreatedAt.Should().Be(expectedCreatedAt);
            }
        }
    }

    public class AddSection
    {
        [Fact]
        public void AddSection_WithSectionAlreadyInShoppingList_ShouldThrowDomainException()
        {
            var shoppingList = ShoppingListMother.OneSectionWithOneItemInBasket().Create();
            var section = CommonFixture.ChooseRandom(shoppingList.Sections);

            // Act
            var action = () => shoppingList.AddSection(section);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.SectionAlreadyInShoppingList);
        }

        [Fact]
        public void AddSection_WithNewSection_ShouldThrowDomainException()
        {
            var shoppingList = ShoppingListMother.OneSectionWithOneItemInBasket().Create();
            var section = new ShoppingListSectionBuilder().Create();

            // Act
            shoppingList.AddSection(section);

            // Assert
            shoppingList.Sections.Should().Contain(section);
        }
    }

    public class TransferItem
    {
        private readonly TransferItemFixture _fixture = new();

        [Fact]
        public void TransferItem_WithoutNewSection_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupSectionIds();
            _fixture.SetupItemIdAndItemTypeId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);

            // Act
            var act = () => sut.TransferItem(_fixture.OldSectionId.Value, _fixture.ItemId.Value, _fixture.ItemTypeId);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.SectionNotFound);
        }

        [Fact]
        public void TransferItem_WithoutItem_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupSectionIds();
            _fixture.SetupItemIdAndItemTypeId();
            _fixture.SetupOldAndNewSectionsWithoutItem();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);

            // Act
            var act = () => sut.TransferItem(_fixture.OldSectionId.Value, _fixture.ItemId.Value, _fixture.ItemTypeId);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotOnShoppingList);
        }

        [Fact]
        public void TransferItem_WithOldSectionEqualsNewSection_ShouldNotTransferItem()
        {
            // Arrange
            _fixture.SetupSectionIds();
            _fixture.SetupItemIdAndItemTypeId();
            _fixture.SetupOldAndNewSections();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);

            // Act
            sut.TransferItem(_fixture.OldSectionId.Value, _fixture.ItemId.Value, _fixture.ItemTypeId);

            // Assert
            var oldSection = sut.Sections.First(s => s.Id == _fixture.OldSectionId.Value);
            var newSection = sut.Sections.First(s => s.Id == _fixture.NewSectionId.Value);

            oldSection.Items.Should().Contain(i =>
                i.Id == _fixture.ItemId.Value && i.TypeId == _fixture.ItemTypeId.Value);
            newSection.Items.Should().NotContain(i =>
                i.Id == _fixture.ItemId.Value && i.TypeId == _fixture.ItemTypeId.Value);
        }

        [Fact]
        public void TransferItem_WithValidData_ShouldTransferItem()
        {
            // Arrange
            _fixture.SetupSectionIds();
            _fixture.SetupItemIdAndItemTypeId();
            _fixture.SetupOldAndNewSections();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);

            // Act
            sut.TransferItem(_fixture.NewSectionId.Value, _fixture.ItemId.Value, _fixture.ItemTypeId);

            // Assert
            var oldSection = sut.Sections.First(s => s.Id == _fixture.OldSectionId.Value);
            var newSection = sut.Sections.First(s => s.Id == _fixture.NewSectionId.Value);

            oldSection.Items.Should().NotContain(i =>
                i.Id == _fixture.ItemId.Value && i.TypeId == _fixture.ItemTypeId.Value);
            newSection.Items.Should().Contain(i =>
                i.Id == _fixture.ItemId.Value && i.TypeId == _fixture.ItemTypeId.Value);
        }

        private class TransferItemFixture
        {
            private readonly ShoppingListBuilder _builder = ShoppingListMother.Initial();

            public SectionId? OldSectionId { get; private set; }
            public SectionId? NewSectionId { get; private set; }
            public ItemId? ItemId { get; private set; }
            public ItemTypeId? ItemTypeId { get; private set; }

            public ShoppingList CreateSut()
            {
                return _builder.Create();
            }

            public void SetupSectionIds()
            {
                OldSectionId = SectionId.New;
                NewSectionId = SectionId.New;
            }

            public void SetupItemIdAndItemTypeId()
            {
                ItemId = Domain.Items.Models.ItemId.New;
                ItemTypeId = Domain.Items.Models.ItemTypeId.New;
            }

            public void SetupOldAndNewSectionsWithoutItem()
            {
                TestPropertyNotSetException.ThrowIfNull(OldSectionId);
                TestPropertyNotSetException.ThrowIfNull(NewSectionId);
                TestPropertyNotSetException.ThrowIfNull(ItemId);

                var sections = new List<IShoppingListSection>
                {
                    new ShoppingListSectionBuilder()
                        .WithId(OldSectionId.Value)
                        .Create(),
                    new ShoppingListSectionBuilder()
                        .WithId(NewSectionId.Value)
                        .Create()
                };

                _builder.WithSections(sections);
            }

            public void SetupOldAndNewSections()
            {
                TestPropertyNotSetException.ThrowIfNull(OldSectionId);
                TestPropertyNotSetException.ThrowIfNull(NewSectionId);
                TestPropertyNotSetException.ThrowIfNull(ItemId);

                var item = new ShoppingListItemBuilder()
                    .WithId(ItemId.Value)
                    .WithTypeId(ItemTypeId)
                    .Create();

                var sections = new List<IShoppingListSection>
                {
                    new ShoppingListSectionBuilder()
                        .WithId(OldSectionId.Value)
                        .WithItem(item)
                        .Create(),
                    new ShoppingListSectionBuilder()
                        .WithId(NewSectionId.Value)
                        .Create()
                };

                _builder.WithSections(sections);
            }
        }
    }

    public class GetDiscountFor
    {
        private readonly GetDiscountForFixture _fixture = new();

        [Fact]
        public void GetDiscountFor_WithoutDiscountForItem_ShouldReturnNull()
        {
            // Arrange
            var sut = _fixture.CreateSut();

            // Act
            var result = sut.GetDiscountFor(_fixture.ItemId, _fixture.ItemTypeId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetDiscountFor_WithDiscountForItemType_ShouldReturnDiscount()
        {
            // Arrange
            _fixture.SetupItemTypeId();
            _fixture.SetupDiscount();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.GetDiscountFor(_fixture.ItemId, _fixture.ItemTypeId);

            // Assert
            result.Should().Be(_fixture.ExpectedResult.Value);
        }

        [Fact]
        public void GetDiscountFor_WithDiscountForItem_ShouldReturnDiscount()
        {
            // Arrange
            _fixture.SetupDiscount();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.GetDiscountFor(_fixture.ItemId, _fixture.ItemTypeId);

            // Assert
            result.Should().Be(_fixture.ExpectedResult.Value);
        }

        private sealed class GetDiscountForFixture : ShoppingListFixture
        {
            public ItemId ItemId { get; private set; } = ItemId.New;
            public ItemTypeId? ItemTypeId { get; private set; }
            public Discount? ExpectedResult { get; private set; }

            public void SetupItemTypeId()
            {
                ItemTypeId = Domain.Items.Models.ItemTypeId.New;
            }

            public void SetupDiscount()
            {
                ExpectedResult = new DiscountBuilder().Create() with
                {
                    ItemId = ItemId,
                    ItemTypeId = ItemTypeId
                };

                Builder.WithDiscounts([ExpectedResult.Value]);
            }
        }
    }

    public class AddDiscount
    {
        private readonly AddDiscountFixture _fixture = new();

        [Fact]
        public void AddDiscount_WithDiscountAlreadyExisting_WithType_ShouldOverwriteExistingDiscount()
        {
            // Arrange
            _fixture.SetupItemTypeId();
            _fixture.SetupDiscount();
            _fixture.SetupItemOnShoppingList();
            _fixture.SetupDiscountAlreadyExisting();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Discount);

            // Act
            sut.AddDiscount(_fixture.Discount.Value);

            // Assert
            sut.Discounts.Should().Contain(_fixture.Discount.Value);
        }

        [Fact]
        public void AddDiscount_WithDiscountAlreadyExisting_WithoutType_ShouldOverwriteExistingDiscount()
        {
            // Arrange
            _fixture.SetupDiscount();
            _fixture.SetupItemOnShoppingList();
            _fixture.SetupDiscountAlreadyExisting();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Discount);

            // Act
            sut.AddDiscount(_fixture.Discount.Value);

            // Assert
            sut.Discounts.Should().Contain(_fixture.Discount.Value);
        }

        [Fact]
        public void AddDiscount_WithItemNotOnShoppingList_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupDiscount();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Discount);

            // Act
            var action = () => sut.AddDiscount(_fixture.Discount.Value);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.ItemNotOnShoppingList);
        }

        [Fact]
        public void AddDiscount_WithDiscountNotAlreadyExisting_WithType_ShouldAddDiscount()
        {
            // Arrange
            _fixture.SetupItemTypeId();
            _fixture.SetupDiscount();
            _fixture.SetupItemOnShoppingList();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Discount);

            // Act
            sut.AddDiscount(_fixture.Discount.Value);

            // Assert
            sut.Discounts.Should().Contain(_fixture.Discount.Value);
        }

        [Fact]
        public void AddDiscount_WithDiscountNotAlreadyExisting_WithoutType_ShouldAddDiscount()
        {
            // Arrange
            _fixture.SetupDiscount();
            _fixture.SetupItemOnShoppingList();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Discount);

            // Act
            sut.AddDiscount(_fixture.Discount.Value);

            // Assert
            sut.Discounts.Should().Contain(_fixture.Discount.Value);
        }

        private sealed class AddDiscountFixture : ShoppingListFixture
        {
            public ItemId ItemId { get; private set; } = ItemId.New;
            public ItemTypeId? ItemTypeId { get; private set; }
            public Discount? Discount { get; private set; }

            public void SetupItemTypeId()
            {
                ItemTypeId = Domain.Items.Models.ItemTypeId.New;
            }

            public void SetupItemOnShoppingList()
            {
                var item = new ShoppingListItemBuilder()
                    .WithId(ItemId)
                    .WithTypeId(ItemTypeId)
                    .Create();

                var section = new ShoppingListSectionBuilder()
                    .WithItem(item)
                    .Create();

                Builder.WithSection(section);
            }

            public void SetupDiscount()
            {
                Discount = new DiscountBuilder().Create() with
                {
                    ItemId = ItemId,
                    ItemTypeId = ItemTypeId
                };
            }

            public void SetupDiscountAlreadyExisting()
            {
                var discount = new DiscountBuilder().Create() with
                {
                    ItemId = ItemId,
                    ItemTypeId = ItemTypeId
                };

                var sut = new ShoppingListBuilder()
                    .WithDiscounts([discount])
                    .Create();

                Builder.WithDiscounts([discount]);
            }
        }
    }

    public class RemoveDiscount
    {
        private readonly RemoveDiscountFixture _fixture = new();

        [Fact]
        public void RemoveDiscount_WithDiscountNotExisting_WithType_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupItemTypeId();
            _fixture.SetupNoMatchingDiscount();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            sut.RemoveDiscount(_fixture.ItemId, _fixture.ItemTypeId);

            // Assert
            sut.Discounts.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void RemoveDiscount_WithDiscountNotExisting_WithoutType_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupNoMatchingDiscount();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            sut.RemoveDiscount(_fixture.ItemId, _fixture.ItemTypeId);

            // Assert
            sut.Discounts.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void RemoveDiscount_WithDiscountExisting_WithType_ShouldRemoveDiscount()
        {
            // Arrange
            _fixture.SetupItemTypeId();
            _fixture.SetupDiscount();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            sut.RemoveDiscount(_fixture.ItemId, _fixture.ItemTypeId);

            // Assert
            sut.Discounts.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void RemoveDiscount_WithDiscountExisting_WithoutType_ShouldRemoveDiscount()
        {
            // Arrange
            _fixture.SetupDiscount();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            sut.RemoveDiscount(_fixture.ItemId, _fixture.ItemTypeId);

            // Assert
            sut.Discounts.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class RemoveDiscountFixture : ShoppingListFixture
        {
            public ItemId ItemId { get; private set; } = ItemId.New;
            public ItemTypeId? ItemTypeId { get; private set; }
            public IReadOnlyCollection<Discount>? ExpectedResult { get; private set; }

            public void SetupItemTypeId()
            {
                ItemTypeId = Domain.Items.Models.ItemTypeId.New;
            }

            public void SetupNoMatchingDiscount()
            {
                ExpectedResult = new DiscountBuilder().CreateMany(2).ToList();

                Builder.WithDiscounts(ExpectedResult);
            }

            public void SetupDiscount()
            {
                ExpectedResult = new DiscountBuilder().CreateMany(2).ToList();

                var discount = new DiscountBuilder().Create() with
                {
                    ItemId = ItemId,
                    ItemTypeId = ItemTypeId
                };

                Builder.WithDiscounts(ExpectedResult.Union([discount]));
            }
        }
    }

    private abstract class ShoppingListFixture
    {
        protected readonly ShoppingListBuilder Builder = new();

        protected ShoppingListFixture()
        {
            Builder.WithoutCompletionDate();
        }

        public ShoppingList CreateSut()
        {
            return Builder.Create();
        }
    }
}