using FluentAssertions.Common;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Shared;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Models.ShoppingLists;

public class ShoppingListTests
{
    private readonly CommonFixture _commonFixture;
    private readonly ShoppingListSectionMockFixture _shoppingListSectionMockFixture;

    public ShoppingListTests()
    {
        _commonFixture = new CommonFixture();
        _shoppingListSectionMockFixture = new ShoppingListSectionMockFixture();
    }

    #region AddItem

    [Fact]
    public void AddItem_WithItemIdIsAlreadyOnList_ShouldThrowDomainException()
    {
        // Arrange
        var shoppingList = ShoppingListMother.ThreeSections().Create();
        var chosenItem = _commonFixture.ChooseRandom(shoppingList.Items);

        var collidingItem = new ShoppingListItemBuilder().WithId(chosenItem.Id).WithTypeId(chosenItem.TypeId).Create();
        SectionId sectionId = new SectionId(Guid.NewGuid());

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
        ShoppingListItem item = new ShoppingListItemBuilder().Create();

        // Act
        Action action = () => shoppingList.AddItem(item, new SectionId(Guid.NewGuid()));

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
        var sectionMocks = _shoppingListSectionMockFixture.CreateMany(3).ToList();
        var shoppingList = ShoppingListMother.Initial()
            .WithSections(sectionMocks.Select(s => s.Object))
            .Create();

        ShoppingListSectionMock chosenSection = _commonFixture.ChooseRandom(sectionMocks);
        ShoppingListItem item = new ShoppingListItemBuilder().Create();

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

        ShoppingListSectionMock chosenSectionMock = _commonFixture.ChooseRandom(sectionMocks);
        ShoppingListItem chosenItem = _commonFixture.ChooseRandom(chosenSectionMock.Object.Items);

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

    #endregion RemoveItem

    #region PutItemInBasket

    [Fact]
    public void PutItemInBasket_WithItemIdNotOnList_ShouldThrowDomainException()
    {
        // Arrange
        var list = ShoppingListMother.ThreeSections().Create();
        var shoppingListItemId = new ItemId(Guid.NewGuid());

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
        var sectionMocks = _shoppingListSectionMockFixture.CreateMany(3).ToList();
        var shoppingList = ShoppingListMother.Initial()
            .WithSections(sectionMocks.Select(s => s.Object))
            .Create();

        ShoppingListSectionMock chosenSectionMock = _commonFixture.ChooseRandom(sectionMocks);
        ShoppingListItem chosenItem = _commonFixture.ChooseRandom(chosenSectionMock.Object.Items);

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

    #endregion PutItemInBasket

    #region RemoveFromBasket

    [Fact]
    public void RemoveFromBasket_WithItemIdNotOnList_ShouldThrowDomainException()
    {
        // Arrange
        var list = ShoppingListMother.ThreeSections().Create();
        var shoppingListItemId = new ItemId(Guid.NewGuid());

        // Act
        Action action = () => list.RemoveFromBasket(shoppingListItemId, null);

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
        var sectionMocks = _shoppingListSectionMockFixture.CreateMany(3).ToList();
        var shoppingList = ShoppingListMother.Initial()
            .WithSections(sectionMocks.Select(s => s.Object))
            .Create();

        ShoppingListSectionMock chosenSectionMock = _commonFixture.ChooseRandom(sectionMocks);
        ShoppingListItem chosenItem = _commonFixture.ChooseRandom(chosenSectionMock.Object.Items);

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

    #endregion RemoveFromBasket

    #region ChangeItemQuantity

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
        using (new AssertionScope())
        {
            action.Should().Throw<DomainException>()
                .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ItemNotOnShoppingList);
        }
    }

    [Fact]
    public void ChangeItemQuantity_WithValidItemId_ShouldChangeQuantity()
    {
        // Arrange
        var sectionMocks = _shoppingListSectionMockFixture.CreateMany(3).ToList();
        var shoppingList = ShoppingListMother.Initial()
            .WithSections(sectionMocks.Select(s => s.Object))
            .Create();

        ShoppingListSectionMock chosenSectionMock = _commonFixture.ChooseRandom(sectionMocks);
        ShoppingListItem chosenItem = _commonFixture.ChooseRandom(chosenSectionMock.Object.Items);

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

    #endregion ChangeItemQuantity

    #region Finish

    [Fact]
    public void Finish_WithCompletedShoppingList_ShouldThrowDomainException()
    {
        // Arrange
        var fixture = _commonFixture.GetNewFixture();
        var shoppingList = ShoppingListMother.Completed().Create();

        DateTimeOffset completionDate = fixture.Create<DateTimeOffset>();

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

        DateTimeOffset completionDate = _commonFixture.GetNewFixture().Create<DateTimeOffset>();

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
    public void AddSection_WithSectionAlreadyInShoppingList_ShouldThrowDomainException()
    {
        IShoppingList shoppingList = ShoppingListMother.OneSectionWithOneItemInBasket().Create();
        IShoppingListSection section = _commonFixture.ChooseRandom(shoppingList.Sections);

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
        IShoppingListSection section = new ShoppingListSectionBuilder().Create();

        // Act
        shoppingList.AddSection(section);

        // Assert
        using (new AssertionScope())
        {
            shoppingList.Sections.Should().Contain(section);
        }
    }

    #endregion AddSection

    public class TransferItem
    {
        private readonly TransferItemFixture _fixture;

        public TransferItem()
        {
            _fixture = new TransferItemFixture();
        }

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

            public Domain.ShoppingLists.Models.ShoppingList CreateSut()
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
}