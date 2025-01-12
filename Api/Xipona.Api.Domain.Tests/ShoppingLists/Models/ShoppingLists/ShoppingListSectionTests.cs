using Force.DeepCloner;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.Xipona.Api.Domain.TestKit.Shared;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.ShoppingLists.Models.ShoppingLists;

public class ShoppingListSectionTests
{
    public class RemoveItem
    {
        public class WithoutType
        {
            private readonly RemoveItemFixture _fixture = new();

            [Fact]
            public void RemoveItem_WithItemInSection_ShouldReturnExpectedResult()
            {
                // Arrange
                _fixture.SetupItemWithoutTypes();
                var sut = _fixture.CreateSut();
                _fixture.SetupExpectedResultForItemWithoutTypes(sut);

                TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);
                TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

                // Act
                var result = sut.RemoveItem(_fixture.ItemId.Value);

                // Assert
                result.Should().BeEquivalentTo(_fixture.ExpectedResult);
            }

            [Fact]
            public void RemoveItem_WithItemNotInSection_ShouldReturnExpectedResult()
            {
                // Arrange
                _fixture.SetupItemWithoutTypes();
                _fixture.SetupInvalidIds();
                var sut = _fixture.CreateSut();
                var expected = sut.DeepClone();

                TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);

                // Act
                var result = sut.RemoveItem(_fixture.ItemId.Value);

                // Assert
                result.Should().BeEquivalentTo(expected);
            }
        }

        public class WithType
        {
            private readonly RemoveItemFixture _fixture = new();

            [Fact]
            public void RemoveItem_WithItemInSection_ShouldReturnExpectedResult()
            {
                // Arrange
                _fixture.SetupItemWithoutTypes();
                var sut = _fixture.CreateSut();
                _fixture.SetupExpectedResultForItemWithoutTypes(sut);

                TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);
                TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

                // Act
                var result = sut.RemoveItem(_fixture.ItemId.Value, null);

                // Assert
                result.Should().BeEquivalentTo(_fixture.ExpectedResult);
            }

            [Fact]
            public void RemoveItem_WithItemNotInSection_ShouldReturnExpectedResult()
            {
                // Arrange
                _fixture.SetupItemWithoutTypes();
                _fixture.SetupInvalidIds();
                var sut = _fixture.CreateSut();
                var expected = sut.DeepClone();

                TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);

                // Act
                var result = sut.RemoveItem(_fixture.ItemId.Value, null);

                // Assert
                result.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void RemoveItem_WithItemTypeInSection_ShouldReturnExpectedResult()
            {
                // Arrange
                _fixture.SetupItemWithTypes();
                var sut = _fixture.CreateSut();
                _fixture.SetupExpectedResultForItemWithTypes(sut);

                TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);
                TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);
                TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

                // Act
                var result = sut.RemoveItem(_fixture.ItemId.Value, _fixture.ItemTypeId.Value);

                // Assert
                result.Should().BeEquivalentTo(_fixture.ExpectedResult);
            }

            [Fact]
            public void RemoveItem_WithItemTypeNotInSection_ShouldReturnExpectedResult()
            {
                // Arrange
                _fixture.SetupItemWithTypes();
                _fixture.SetupInvalidIds();
                var sut = _fixture.CreateSut();
                var expected = sut.DeepClone();

                TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);
                TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);

                // Act
                var result = sut.RemoveItem(_fixture.ItemId.Value, _fixture.ItemTypeId.Value);

                // Assert
                result.Should().BeEquivalentTo(expected);
            }
        }

        private sealed class RemoveItemFixture : ShoppingListSectionFixture
        {
            public ShoppingListSection? ExpectedResult { get; private set; }
            public ItemId? ItemId { get; private set; }
            public ItemTypeId? ItemTypeId { get; private set; }

            public void SetupItemWithoutTypes()
            {
                var items = new ShoppingListItemBuilder().WithoutTypeId().CreateMany(3).ToList();
                ItemId = items.First().Id;
                SetupItems(items);
            }

            public void SetupItemWithTypes()
            {
                var items = new ShoppingListItemBuilder().CreateMany(3).ToList();
                var item = items.First();
                ItemId = item.Id;
                ItemTypeId = item.TypeId;
                SetupItems(items);
            }

            public void SetupInvalidIds()
            {
                ItemId = Domain.Items.Models.ItemId.New;
                ItemTypeId = Domain.Items.Models.ItemTypeId.New;
            }

            public void SetupExpectedResultForItemWithoutTypes(ShoppingListSection sut)
            {
                TestPropertyNotSetException.ThrowIfNull(ItemId);

                var items = sut.Items.Where(i => i.Id != ItemId).ToList();
                ExpectedResult = new ShoppingListSectionBuilder(sut).WithItems(items).Create();
            }

            public void SetupExpectedResultForItemWithTypes(ShoppingListSection sut)
            {
                TestPropertyNotSetException.ThrowIfNull(ItemId);
                TestPropertyNotSetException.ThrowIfNull(ItemTypeId);

                var items = sut.Items.Where(i => i.Id != ItemId || i.TypeId != ItemTypeId).ToList();
                ExpectedResult = new ShoppingListSectionBuilder(sut).WithItems(items).Create();
            }
        }
    }

    public class ContainsItem
    {
        public class WithoutType
        {
            private readonly ContainsItemFixture _fixture = new();

            [Fact]
            public void ContainsItem_WithItemInSection_ShouldReturnTrue()
            {
                // Arrange
                _fixture.SetupItemWithoutTypes();
                var sut = _fixture.CreateSut();

                TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);

                // Act
                var result = sut.ContainsItem(_fixture.ItemId.Value);

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public void ContainsItem_WithItemNotInSection_ShouldReturnFalse()
            {
                // Arrange
                _fixture.SetupItemWithoutTypes();
                _fixture.SetupInvalidIds();
                var sut = _fixture.CreateSut();

                TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);

                // Act
                var result = sut.ContainsItem(_fixture.ItemId.Value);

                // Assert
                result.Should().BeFalse();
            }
        }

        public class WithType
        {
            private readonly ContainsItemFixture _fixture = new();

            [Fact]
            public void ContainsItem_WithItemInSection_ShouldReturnTrue()
            {
                // Arrange
                _fixture.SetupItemWithoutTypes();
                var sut = _fixture.CreateSut();

                TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);

                // Act
                var result = sut.ContainsItem(_fixture.ItemId.Value);

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public void ContainsItem_WithItemNotInSection_ShouldReturnFalse()
            {
                // Arrange
                _fixture.SetupItemWithoutTypes();
                _fixture.SetupInvalidIds();
                var sut = _fixture.CreateSut();

                TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);

                // Act
                var result = sut.ContainsItem(_fixture.ItemId.Value, null);

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public void ContainsItem_WithItemTypeInSection_ShouldReturnTrue()
            {
                // Arrange
                _fixture.SetupItemWithTypes();
                var sut = _fixture.CreateSut();

                TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);
                TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);

                // Act
                var result = sut.ContainsItem(_fixture.ItemId.Value, _fixture.ItemTypeId.Value);

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public void ContainsItem_WithItemTypeNotInSection_ShouldReturnFalse()
            {
                // Arrange
                _fixture.SetupItemWithTypes();
                _fixture.SetupInvalidIds();
                var sut = _fixture.CreateSut();

                TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);
                TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);

                // Act
                var result = sut.ContainsItem(_fixture.ItemId.Value, _fixture.ItemTypeId.Value);

                // Assert
                result.Should().BeFalse();
            }
        }

        private sealed class ContainsItemFixture : ShoppingListSectionFixture
        {
            public ItemId? ItemId { get; private set; }
            public ItemTypeId? ItemTypeId { get; private set; }

            public void SetupItemWithoutTypes()
            {
                var items = new ShoppingListItemBuilder().WithoutTypeId().CreateMany(3).ToList();
                ItemId = CommonFixture.ChooseRandom(items).Id;
                SetupItems(items);
            }

            public void SetupItemWithTypes()
            {
                var items = new ShoppingListItemBuilder().CreateMany(3).ToList();
                var item = CommonFixture.ChooseRandom(items);
                ItemId = item.Id;
                ItemTypeId = item.TypeId;
                SetupItems(items);
            }

            public void SetupInvalidIds()
            {
                ItemId = Domain.Items.Models.ItemId.New;
                ItemTypeId = Domain.Items.Models.ItemTypeId.New;
            }
        }
    }

    public class AddItem
    {
        private readonly AddItemFixture _fixture = new();

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddItem_WithItemNotAlreadyExisting_ShouldReturnExpectedResult(bool throwIfAlreadyPresent)
        {
            // Arrange
            _fixture.SetupItemNotAlreadyExisting();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultForNotAlreadyExisting(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.AddItem(_fixture.Item, throwIfAlreadyPresent);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void AddItem_WithItemAlreadyExisting_WithNotThrowIfAlreadyPresent_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemAlreadyExisting();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultForItemAlreadyExisting(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.AddItem(_fixture.Item, false);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void AddItem_WithItemAlreadyExisting_WithThrowIfAlreadyPresent_ShouldThrowException()
        {
            // Arrange
            _fixture.SetupItemAlreadyExisting();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            Action act = () => sut.AddItem(_fixture.Item);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemAlreadyInSection);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddItem_WithItemWithTypeNotAlreadyExisting_ShouldReturnExpectedResult(bool throwIfAlreadyPresent)
        {
            // Arrange
            _fixture.SetupItemWithTypeNotAlreadyExisting();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultForNotAlreadyExisting(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.AddItem(_fixture.Item, throwIfAlreadyPresent);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void AddItem_WithItemWithTypeAlreadyExisting_WithNotThrowIfAlreadyPresent_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemWithTypeAlreadyExisting();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultForItemWithTypeAlreadyExisting(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.AddItem(_fixture.Item, false);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void AddItem_WithItemWithTypeAlreadyExisting_WithThrowIfAlreadyPresent_ShouldThrowException()
        {
            // Arrange
            _fixture.SetupItemWithTypeAlreadyExisting();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            Action act = () => sut.AddItem(_fixture.Item);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemAlreadyInSection);
        }

        private sealed class AddItemFixture : ShoppingListSectionFixture
        {
            public ShoppingListItem? Item { get; private set; }
            public ShoppingListSection? ExpectedResult { get; private set; }

            public void SetupItemNotAlreadyExisting()
            {
                Item = ShoppingListItemMother.NotInBasket().WithoutTypeId().Create();
            }

            public void SetupItemAlreadyExisting()
            {
                Item = ShoppingListItemMother.NotInBasket().WithoutTypeId().Create();
                var existingItem = ShoppingListItemMother.NotInBasket().WithId(Item.Id).WithoutTypeId().Create();
                SetupItem(existingItem);
            }

            public void SetupItemWithTypeNotAlreadyExisting()
            {
                Item = ShoppingListItemMother.NotInBasket().Create();
            }

            public void SetupItemWithTypeAlreadyExisting()
            {
                Item = ShoppingListItemMother.NotInBasket().Create();
                var existingItem = ShoppingListItemMother.NotInBasket().WithId(Item.Id).WithTypeId(Item.TypeId).Create();
                SetupItem(existingItem);
            }

            public void SetupExpectedResultForItemAlreadyExisting(ShoppingListSection sut)
            {
                TestPropertyNotSetException.ThrowIfNull(Item);

                var withoutExistingItem = sut.Items.Where(i => i.Id != Item!.Id).ToList();
                var existingItem = sut.Items.First(i => i.Id == Item!.Id);
                withoutExistingItem.Add(new ShoppingListItem(Item.Id, null, existingItem.IsInBasket,
                    existingItem.Quantity + Item.Quantity));

                ExpectedResult = new ShoppingListSection(sut.Id, withoutExistingItem);
            }

            public void SetupExpectedResultForNotAlreadyExisting(ShoppingListSection sut)
            {
                TestPropertyNotSetException.ThrowIfNull(Item);

                ExpectedResult = new ShoppingListSection(sut.Id, sut.Items.Append(Item));
            }

            public void SetupExpectedResultForItemWithTypeAlreadyExisting(ShoppingListSection sut)
            {
                TestPropertyNotSetException.ThrowIfNull(Item);

                var withoutExistingItem = sut.Items.Where(i => i.Id != Item!.Id && i.TypeId != Item.TypeId).ToList();
                var existingItem = sut.Items.First(i => i.Id == Item!.Id && i.TypeId == Item.TypeId);
                withoutExistingItem.Add(new ShoppingListItem(Item.Id, Item.TypeId, existingItem.IsInBasket,
                    existingItem.Quantity + Item.Quantity));

                ExpectedResult = new ShoppingListSection(sut.Id, withoutExistingItem);
            }
        }
    }

    public class PutItemInBasket
    {
        private readonly PutItemInBasketFixture _fixture = new();

        [Fact]
        public void PutItemInBasket_WithItemNotAlreadyInBasket_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemWithTypeNotAlreadyInBasket();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            var result = sut.PutItemInBasket(_fixture.Item.Id, _fixture.Item.TypeId);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void PutItemInBasket_WithItemAlreadyInBasket_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemWithTypeAlreadyInBasket();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            var result = sut.PutItemInBasket(_fixture.Item.Id, _fixture.Item.TypeId);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void PutItemInBasket_WithItemTypeIdNotProvided_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemWithTypeAlreadyInBasket();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            var act = () => sut.PutItemInBasket(_fixture.Item.Id, null);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotInSection);
        }

        [Fact]
        public void PutItemInBasket_WithWrongItemTypeIdProvided_ShouldThrowException()
        {
            // Arrange
            _fixture.SetupItemNotAlreadyInBasket();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            Action act = () => sut.PutItemInBasket(_fixture.Item.Id, ItemTypeId.New);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotInSection);
        }

        [Fact]
        public void PutItemInBasket_WithWrongItemIdProvided_ShouldThrowException()
        {
            // Arrange
            _fixture.SetupItemNotAlreadyInBasket();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            Action act = () => sut.PutItemInBasket(ItemId.New, null);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotInSection);
        }

        private sealed class PutItemInBasketFixture : ShoppingListSectionFixture
        {
            public ShoppingListSection? ExpectedResult { get; private set; }
            public ShoppingListItem? Item { get; private set; }

            public void SetupItemNotAlreadyInBasket()
            {
                Item = ShoppingListItemMother.NotInBasket().WithoutTypeId().Create();
                SetupItem(Item);
            }

            public void SetupItemWithTypeNotAlreadyInBasket()
            {
                Item = ShoppingListItemMother.NotInBasket().Create();
                SetupItem(Item);
            }

            public void SetupItemWithTypeAlreadyInBasket()
            {
                Item = ShoppingListItemMother.InBasket().Create();
                SetupItem(Item);
            }

            public void SetupExpectedResult(ShoppingListSection sut)
            {
                var item = sut.Items.First();
                var newItem = new ShoppingListItem(item.Id, item.TypeId, true, item.Quantity);

                ExpectedResult = new ShoppingListSection(sut.Id, [newItem]);
            }
        }
    }

    public class RemoveItemFromBasket
    {
        private readonly RemoveItemFromBasketFixture _fixture = new();

        [Fact]
        public void RemoveItemFromBasket_WithItemNotAlreadyInBasket_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemWithTypeNotAlreadyInBasket();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            var result = sut.RemoveItemFromBasket(_fixture.Item.Id, _fixture.Item.TypeId);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void RemoveItemFromBasket_WithItemAlreadyInBasket_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemWithTypeAlreadyInBasket();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            var result = sut.RemoveItemFromBasket(_fixture.Item.Id, _fixture.Item.TypeId);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void RemoveItemFromBasket_WithItemTypeIdNotProvided_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemWithTypeAlreadyInBasket();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            var act = () => sut.RemoveItemFromBasket(_fixture.Item.Id, null);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotInSection);
        }

        [Fact]
        public void RemoveItemFromBasket_WithWrongItemTypeIdProvided_ShouldThrowException()
        {
            // Arrange
            _fixture.SetupItemNotAlreadyInBasket();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            Action act = () => sut.RemoveItemFromBasket(_fixture.Item.Id, ItemTypeId.New);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotInSection);
        }

        [Fact]
        public void RemoveItemFromBasket_WithWrongItemIdProvided_ShouldThrowException()
        {
            // Arrange
            _fixture.SetupItemNotAlreadyInBasket();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            Action act = () => sut.RemoveItemFromBasket(ItemId.New, null);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotInSection);
        }

        private sealed class RemoveItemFromBasketFixture : ShoppingListSectionFixture
        {
            public ShoppingListSection? ExpectedResult { get; private set; }
            public ShoppingListItem? Item { get; private set; }

            public void SetupItemNotAlreadyInBasket()
            {
                Item = ShoppingListItemMother.NotInBasket().WithoutTypeId().Create();
                SetupItem(Item);
            }

            public void SetupItemWithTypeNotAlreadyInBasket()
            {
                Item = ShoppingListItemMother.NotInBasket().Create();
                SetupItem(Item);
            }

            public void SetupItemWithTypeAlreadyInBasket()
            {
                Item = ShoppingListItemMother.InBasket().Create();
                SetupItem(Item);
            }

            public void SetupExpectedResult(ShoppingListSection sut)
            {
                var item = sut.Items.First();
                var newItem = new ShoppingListItem(item.Id, item.TypeId, false, item.Quantity);

                ExpectedResult = new ShoppingListSection(sut.Id, [newItem]);
            }
        }
    }

    public class ChangeItemQuantity
    {
        private readonly ChangeItemQuantityFixture _fixture = new();

        [Fact]
        public void ChangeItemQuantity_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemQuantity();
            _fixture.SetupItemWithType();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Quantity);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            var result = sut.ChangeItemQuantity(_fixture.Item.Id, _fixture.Item.TypeId, _fixture.Quantity.Value);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void ChangeItemQuantity_WithItemTypeIdNotProvided_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemQuantity();
            _fixture.SetupItemWithType();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Quantity);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            var act = () => sut.ChangeItemQuantity(_fixture.Item.Id, null, _fixture.Quantity.Value);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotInSection);
        }

        [Fact]
        public void ChangeItemQuantity_WithWrongItemTypeIdProvided_ShouldThrowException()
        {
            // Arrange
            _fixture.SetupItemQuantity();
            _fixture.SetupItem();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Quantity);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            Action act = () => sut.ChangeItemQuantity(_fixture.Item.Id, ItemTypeId.New, _fixture.Quantity.Value);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotInSection);
        }

        [Fact]
        public void ChangeItemQuantity_WithWrongItemIdProvided_ShouldThrowException()
        {
            // Arrange
            _fixture.SetupItemQuantity();
            _fixture.SetupItem();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Quantity);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            Action act = () => sut.ChangeItemQuantity(ItemId.New, null, _fixture.Quantity.Value);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotInSection);
        }

        private sealed class ChangeItemQuantityFixture : ShoppingListSectionFixture
        {
            public ShoppingListSection? ExpectedResult { get; private set; }
            public ShoppingListItem? Item { get; private set; }
            public QuantityInBasket? Quantity { get; private set; }

            public void SetupItemQuantity()
            {
                Quantity = new DomainTestBuilder<QuantityInBasket>().Create();
            }

            public void SetupItem()
            {
                Item = new ShoppingListItemBuilder().WithoutTypeId().Create();
                SetupItem(Item);
            }

            public void SetupItemWithType()
            {
                Item = new ShoppingListItemBuilder().Create();
                SetupItem(Item);
            }

            public void SetupExpectedResult(ShoppingListSection sut)
            {
                TestPropertyNotSetException.ThrowIfNull(Quantity);

                var item = sut.Items.First();
                var newItem = new ShoppingListItem(item.Id, item.TypeId, item.IsInBasket, Quantity.Value);

                ExpectedResult = new ShoppingListSection(sut.Id, [newItem]);
            }
        }
    }

    public class RemoveItemsNotInBasket
    {
        private readonly RemoveItemsNotInBasketFixture _fixture = new();

        [Fact]
        public void RemoveItemsNotInBasket_WithItemsInBasket_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemsInBasket();
            _fixture.SetupItemsNotInBasket();
            _fixture.SetupItemsInSection();
            var sut = _fixture.CreateSut();

            // Act
            var result = sut.RemoveItemsNotInBasket();

            // Assert
            result.Items.Should().BeEquivalentTo(_fixture.ItemsInBasket);
            sut.Items.Should().Contain(_fixture.ItemsInBasket);
            sut.Items.Should().Contain(_fixture.ItemsNotInBasket);
        }

        [Fact]
        public void RemoveItemsNotInBasket_WithNoItemsInBasket_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemsNotInBasket();
            _fixture.SetupItemsInSection();
            var sut = _fixture.CreateSut();

            // Act
            var result = sut.RemoveItemsNotInBasket();

            // Assert
            result.Items.Should().BeEmpty();
            sut.Items.Should().BeEquivalentTo(_fixture.ItemsNotInBasket);
        }

        [Fact]
        public void RemoveItemsNotInBasket_WithAllItemsInBasket_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemsInBasket();
            _fixture.SetupItemsInSection();
            var sut = _fixture.CreateSut();

            // Act
            var result = sut.RemoveItemsNotInBasket();

            // Assert
            result.Items.Should().BeEquivalentTo(_fixture.ItemsInBasket);
            sut.Items.Should().BeEquivalentTo(_fixture.ItemsInBasket);
        }

        private sealed class RemoveItemsNotInBasketFixture : ShoppingListSectionFixture
        {
            public IReadOnlyCollection<ShoppingListItem> ItemsInBasket { get; private set; } = new List<ShoppingListItem>(0);
            public IReadOnlyCollection<ShoppingListItem> ItemsNotInBasket { get; private set; } = new List<ShoppingListItem>(0);

            public void SetupItemsInBasket()
            {
                ItemsInBasket = ShoppingListItemMother.InBasket().CreateMany(3).ToList();
            }

            public void SetupItemsNotInBasket()
            {
                ItemsNotInBasket = ShoppingListItemMother.NotInBasket().CreateMany(3).ToList();
            }

            public void SetupItemsInSection()
            {
                var items = ItemsInBasket.Union(ItemsNotInBasket);
                SetupItems(items);
            }
        }
    }

    public class RemoveItemsInBasket
    {
        private readonly RemoveItemsInBasketFixture _fixture = new();

        [Fact]
        public void RemoveItemsInBasket_WithItemsNotInBasket_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemsInBasket();
            _fixture.SetupItemsNotInBasket();
            _fixture.SetupItemsInSection();
            var sut = _fixture.CreateSut();

            // Act
            var result = sut.RemoveItemsInBasket();

            // Assert
            result.Items.Should().BeEquivalentTo(_fixture.ItemsNotInBasket);
            sut.Items.Should().Contain(_fixture.ItemsInBasket);
            sut.Items.Should().Contain(_fixture.ItemsNotInBasket);
        }

        [Fact]
        public void RemoveItemsInBasket_WithNoItemsInBasket_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemsNotInBasket();
            _fixture.SetupItemsInSection();
            var sut = _fixture.CreateSut();

            // Act
            var result = sut.RemoveItemsInBasket();

            // Assert
            result.Items.Should().BeEquivalentTo(_fixture.ItemsNotInBasket);
            sut.Items.Should().BeEquivalentTo(_fixture.ItemsNotInBasket);
        }

        [Fact]
        public void RemoveItemsInBasket_WithAllItemsInBasket_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemsInBasket();
            _fixture.SetupItemsInSection();
            var sut = _fixture.CreateSut();

            // Act
            var result = sut.RemoveItemsInBasket();

            // Assert
            result.Items.Should().BeEmpty();
            sut.Items.Should().BeEquivalentTo(_fixture.ItemsInBasket);
        }

        private sealed class RemoveItemsInBasketFixture : ShoppingListSectionFixture
        {
            public IReadOnlyCollection<ShoppingListItem> ItemsInBasket { get; private set; } = new List<ShoppingListItem>(0);
            public IReadOnlyCollection<ShoppingListItem> ItemsNotInBasket { get; private set; } = new List<ShoppingListItem>(0);

            public void SetupItemsInBasket()
            {
                ItemsInBasket = ShoppingListItemMother.InBasket().CreateMany(3).ToList();
            }

            public void SetupItemsNotInBasket()
            {
                ItemsNotInBasket = ShoppingListItemMother.NotInBasket().CreateMany(3).ToList();
            }

            public void SetupItemsInSection()
            {
                var items = ItemsInBasket.Union(ItemsNotInBasket);
                SetupItems(items);
            }
        }
    }

    private abstract class ShoppingListSectionFixture
    {
        private readonly ShoppingListSectionBuilder _builder = new();

        protected void SetupItem(ShoppingListItem item)
        {
            _builder.WithItem(item);
        }

        protected void SetupItems(IEnumerable<ShoppingListItem> items)
        {
            _builder.WithItems(items);
        }

        public ShoppingListSection CreateSut()
        {
            return _builder.Create();
        }
    }
}