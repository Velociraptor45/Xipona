using Force.DeepCloner;
using ProjectHermes.Xipona.Api.Domain.Items.DomainEvents;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models.Factories;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using ProjectHermes.Xipona.Api.TestTools.Extensions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.Items.Models;

public class ItemTypesTests
{
    public sealed class Update
    {
        private readonly UpdateFixture _fixture = new();

        public static IEnumerable<object?[]> GetUpdatePriceItemTypeIdCombinations()
        {
            yield return new object?[] { null, ItemTypeId.New };

            var itemTypeId = ItemTypeId.New;
            yield return new object?[] { itemTypeId, itemTypeId };
        }

        public static IEnumerable<object?[]> GetUpdatePriceItemTypeIdForTypeNotAvailable()
        {
            yield return new object?[] { null };
            yield return new object?[] { ItemTypeId.New };
        }

        [Theory]
        [MemberData(nameof(GetUpdatePriceItemTypeIdForTypeNotAvailable))]
        public void Update_WithItemTypeNotAvailableAtStore_ShouldCallCorrectMethod(ItemTypeId? itemTypeIdArg)
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupPrice();
            _fixture.SetupItemTypeId(itemTypeIdArg);
            _fixture.SetupItemTypeMockNotAvailableAtStore();
            _fixture.SetupCallingUpdate();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value);

            // Assert
            _fixture.VerifyCallingUpdate();
        }

        [Theory]
        [MemberData(nameof(GetUpdatePriceItemTypeIdForTypeNotAvailable))]
        public void Update_WithItemTypeNotAvailableAtStore_ShouldReturnExpectedResult(ItemTypeId? itemTypeIdArg)
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupPrice();
            _fixture.SetupItemTypeId(itemTypeIdArg);
            _fixture.SetupItemTypeMockNotAvailableAtStore();
            _fixture.SetupCallingUpdate();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var result = sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedItemTypes);
        }

        [Fact]
        public void Update_WithItemTypeAvailableAtStoreButTypeIdNotMatching_ShouldCallCorrectMethod()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupPrice();
            _fixture.SetupItemTypeId(ItemTypeId.New);
            _fixture.SetupItemTypeMockAvailableAtStore(ItemTypeId.New);
            _fixture.SetupCallingUpdate();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value);

            // Assert
            _fixture.VerifyCallingUpdate();
        }

        [Fact]
        public void Update_WithItemTypeAvailableAtStoreButTypeIdNotMatching_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupPrice();
            _fixture.SetupItemTypeId(ItemTypeId.New);
            _fixture.SetupItemTypeMockAvailableAtStore(ItemTypeId.New);
            _fixture.SetupCallingUpdate();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var result = sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedItemTypes);
        }

        [Theory]
        [MemberData(nameof(GetUpdatePriceItemTypeIdCombinations))]
        public void Update_WithItemTypeAvailableAtStore_ShouldCallCorrectMethod(ItemTypeId? itemTypeIdArg,
            ItemTypeId itemTypeId)
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupPrice();
            _fixture.SetupItemTypeId(itemTypeIdArg);
            _fixture.SetupItemTypeMockAvailableAtStore(itemTypeId);
            _fixture.SetupCallingUpdateWithPrice();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value);

            // Assert
            _fixture.VerifyCallingUpdateWithPrice();
        }

        [Theory]
        [MemberData(nameof(GetUpdatePriceItemTypeIdCombinations))]
        public void Update_WithItemTypeAvailableAtStore_ShouldReturnExpectedResult(ItemTypeId? itemTypeIdArg,
            ItemTypeId itemTypeId)
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupPrice();
            _fixture.SetupItemTypeId(itemTypeIdArg);
            _fixture.SetupItemTypeMockAvailableAtStore(itemTypeId);
            _fixture.SetupCallingUpdateWithPrice();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var result = sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedItemTypes);
        }

        private sealed class UpdateFixture
        {
            private readonly ItemTypeFactoryMock _itemTypeFactoryMock = new(MockBehavior.Strict);
            private List<ItemTypeMock>? _itemTypeMocks;
            public Price? Price { get; private set; }
            public StoreId? StoreId { get; private set; }
            public ItemTypeId? ItemTypeId { get; private set; }
            public IReadOnlyCollection<ItemType>? ExpectedItemTypes { get; private set; }

            public ItemTypes CreateSut()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemTypeMocks);

                return new ItemTypes(_itemTypeMocks.Select(m => m.Object), _itemTypeFactoryMock.Object);
            }

            public void SetupStoreId()
            {
                StoreId = Domain.Stores.Models.StoreId.New;
            }

            public void SetupItemTypeId(ItemTypeId? itemTypeId)
            {
                ItemTypeId = itemTypeId;
            }

            public void SetupPrice()
            {
                Price = new DomainTestBuilder<Price>().Create();
            }

            public void SetupItemTypeMockAvailableAtStore(ItemTypeId itemTypeId)
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                var itemType = new ItemTypeBuilder().WithId(itemTypeId).Create();
                _itemTypeMocks = new() { new ItemTypeMock(itemType, MockBehavior.Strict) };
                _itemTypeMocks.First().SetupIsAvailableAtStore(StoreId.Value, true);
            }

            public void SetupItemTypeMockNotAvailableAtStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                _itemTypeMocks = new() { new ItemTypeMock(new ItemTypeBuilder().Create(), MockBehavior.Strict) };
                _itemTypeMocks.First().SetupIsAvailableAtStore(StoreId.Value, false);
            }

            public void SetupCallingUpdateWithPrice()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);
                TestPropertyNotSetException.ThrowIfNull(Price);
                TestPropertyNotSetException.ThrowIfNull(_itemTypeMocks);

                ExpectedItemTypes = new ItemTypeBuilder().CreateMany(1).ToList();
                _itemTypeMocks.First().SetupUpdate(StoreId.Value, Price.Value, ExpectedItemTypes.First());
            }

            public void SetupCallingUpdate()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemTypeMocks);

                ExpectedItemTypes = new ItemTypeBuilder().CreateMany(1).ToList();
                _itemTypeMocks.First().SetupUpdate(ExpectedItemTypes.First());
            }

            public void VerifyCallingUpdateWithPrice()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);
                TestPropertyNotSetException.ThrowIfNull(Price);
                TestPropertyNotSetException.ThrowIfNull(_itemTypeMocks);

                _itemTypeMocks.First().VerifyUpdate(StoreId.Value, Price.Value, Times.Once);
            }

            public void VerifyCallingUpdate()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemTypeMocks);

                _itemTypeMocks.First().VerifyUpdate(Times.Once);
            }
        }
    }

    public sealed class TransferToDefaultSection
    {
        private readonly TransferToDefaultSectionFixture _fixture = new();

        [Fact]
        public void TransferToDefaultSection_WithItemTypeInOldSection_ShouldTransferToNewSection()
        {
            // Arrange
            _fixture.SetupOldSectionId();
            _fixture.SetupNewSectionId();
            _fixture.SetupItemTypeInOldSection();
            _fixture.SetupExpectedTypeInNewSection();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedType);

            // Act
            sut.TransferToDefaultSection(_fixture.OldSectionId.Value, _fixture.NewSectionId.Value);

            // Assert
            var types = sut.ToList();
            types.Should().HaveCount(1);
            types[0].Should().BeEquivalentTo(_fixture.ExpectedType);
        }

        [Fact]
        public void TransferToDefaultSection_WithItemTypeNotInOldSection_ShouldNotTransferToNewSection()
        {
            // Arrange
            _fixture.SetupOldSectionId();
            _fixture.SetupNewSectionId();
            _fixture.SetupItemTypeNotInOldSection();
            _fixture.SetupExpectedTypeNotInNewSection();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedType);

            // Act
            sut.TransferToDefaultSection(_fixture.OldSectionId.Value, _fixture.NewSectionId.Value);

            // Assert
            var types = sut.ToList();
            types.Should().HaveCount(1);
            types[0].Should().BeEquivalentTo(_fixture.ExpectedType);
        }

        private sealed class TransferToDefaultSectionFixture : ItemTypesFixture
        {
            public SectionId? OldSectionId { get; private set; }
            public SectionId? NewSectionId { get; private set; }
            public ItemType? ExpectedType { get; private set; }

            public void SetupOldSectionId()
            {
                OldSectionId = SectionId.New;
            }

            public void SetupNewSectionId()
            {
                NewSectionId = SectionId.New;
            }

            public void SetupItemTypeInOldSection()
            {
                TestPropertyNotSetException.ThrowIfNull(OldSectionId);

                var itemType = ItemTypeMother.InitialAvailableAt(OldSectionId.Value).Create();
                ItemTypes.Add(itemType);
            }

            public void SetupItemTypeNotInOldSection()
            {
                var itemType = ItemTypeMother.Initial().Create();
                ItemTypes.Add(itemType);
            }

            public void SetupExpectedTypeInNewSection()
            {
                TestPropertyNotSetException.ThrowIfNull(NewSectionId);

                var type = ItemTypes[0];

                var av = new ItemAvailabilityBuilder()
                    .WithDefaultSectionId(NewSectionId.Value)
                    .WithPrice(type.Availabilities.First().Price)
                    .WithStoreId(type.Availabilities.First().StoreId)
                    .Create();

                ExpectedType = new ItemTypeBuilder()
                    .WithAvailability(av)
                    .WithId(type.Id)
                    .WithName(type.Name)
                    .WithIsDeleted(type.IsDeleted)
                    .WithPredecessorId(type.PredecessorId)
                    .WithCreatedAt(type.CreatedAt)
                    .Create();
            }

            public void SetupExpectedTypeNotInNewSection()
            {
                ExpectedType = ItemTypes[0].DeepClone();
            }
        }
    }

    public sealed class RemoveAvailabilitiesFor
    {
        private readonly RemoveAvailabilitiesForFixture _fixture = new();

        [Fact]
        public void RemoveAvailabilitiesFor_WithItemTypeNotAvailableAtStore_ShouldNotRemoveAvailability()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeNotAvailableAtStore();
            _fixture.SetupExpectedResultUnchanged();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedType);

            // Act
            sut.RemoveAvailabilitiesFor(_fixture.StoreId.Value, out var domainEvents);

            // Assert
            var types = sut.ToList();
            types.Should().HaveCount(1);
            types[0].Should().BeEquivalentTo(_fixture.ExpectedType);

            var domainEventsList = domainEvents.ToList();
            domainEventsList.Should().BeEmpty();
        }

        [Fact]
        public void RemoveAvailabilitiesFor_WithItemTypeOnlyAvailableAtStore_ShouldDeleteItemType()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeOnlyAvailableAtStore();
            _fixture.SetupExpectedResultDeleted();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedType);

            // Act
            sut.RemoveAvailabilitiesFor(_fixture.StoreId.Value, out var domainEvents);

            // Assert
            var types = sut.ToList();
            types.Should().HaveCount(1);
            types[0].Should().BeEquivalentTo(_fixture.ExpectedType);

            var domainEventsList = domainEvents.ToList();
            domainEventsList.Should().HaveCount(1);
            domainEventsList[0].Should().BeOfType<ItemTypeDeletedDomainEvent>();
        }

        [Fact]
        public void RemoveAvailabilitiesFor_WithItemTypeAvailableAtStoreAndOthers_ShouldRemoveAvailability()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeAvailableAtStoreAndOthers();
            _fixture.SetupExpectedResultWithoutStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedType);

            // Act
            sut.RemoveAvailabilitiesFor(_fixture.StoreId.Value, out var domainEvents);

            // Assert
            var types = sut.ToList();
            types.Should().HaveCount(1);
            types[0].Should().BeEquivalentTo(_fixture.ExpectedType);

            var domainEventsList = domainEvents.ToList();
            domainEventsList.Should().HaveCount(1);
            domainEventsList[0].Should().BeOfType<ItemTypeAvailabilityDeletedDomainEvent>();
        }

        private sealed class RemoveAvailabilitiesForFixture : ItemTypesFixture
        {
            public StoreId? StoreId { get; private set; }
            public ItemType? ExpectedType { get; private set; }

            public void SetupStoreId()
            {
                StoreId = Domain.Stores.Models.StoreId.New;
            }

            public void SetupItemTypeNotAvailableAtStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                var itemType = ItemTypeMother.Initial().Create();
                ItemTypes.Add(itemType);
            }

            public void SetupItemTypeOnlyAvailableAtStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                var itemType = ItemTypeMother.InitialAvailableAt(StoreId.Value).Create();
                ItemTypes.Add(itemType);
            }

            public void SetupItemTypeAvailableAtStoreAndOthers()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                var availabilities = new List<ItemAvailability>()
                {
                    ItemAvailabilityMother.ForStore(StoreId.Value).Create(),
                    ItemAvailabilityMother.Initial().Create()
                };
                availabilities.Shuffle();
                var itemType = ItemTypeMother.Initial().WithAvailabilities(availabilities).Create();
                ItemTypes.Add(itemType);
            }

            public void SetupExpectedResultUnchanged()
            {
                var type = ItemTypes[0];

                ExpectedType = type.DeepClone();
            }

            public void SetupExpectedResultDeleted()
            {
                var type = ItemTypes[0];

                ExpectedType = new ItemTypeBuilder()
                    .WithAvailability(type.Availabilities.First())
                    .WithId(type.Id)
                    .WithName(type.Name)
                    .WithIsDeleted(true)
                    .WithPredecessorId(type.PredecessorId)
                    .WithCreatedAt(type.CreatedAt)
                    .Create();
            }

            public void SetupExpectedResultWithoutStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                var type = ItemTypes[0];

                ExpectedType = new ItemTypeBuilder()
                    .WithAvailability(type.Availabilities.First(av => av.StoreId != StoreId.Value))
                    .WithId(type.Id)
                    .WithName(type.Name)
                    .WithIsDeleted(type.IsDeleted)
                    .WithPredecessorId(type.PredecessorId)
                    .WithCreatedAt(type.CreatedAt)
                    .Create();
            }
        }
    }

    private abstract class ItemTypesFixture
    {
        protected IList<ItemType> ItemTypes { get; } = new List<ItemType>();
        protected ItemTypeFactoryMock ItemTypeFactoryMock { get; } = new(MockBehavior.Strict);

        public ItemTypes CreateSut()
        {
            return new ItemTypes(ItemTypes, ItemTypeFactoryMock.Object);
        }
    }
}