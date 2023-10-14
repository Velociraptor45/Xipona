using Force.DeepCloner;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.TestKit.Services;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.TemporaryItems;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Shared;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System.Text.RegularExpressions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.Models;

public class ItemTests
{
    public class Delete
    {
        [Fact]
        public void Delete_WithNotDeletedItem_ShouldMarkItemAsDeleted()
        {
            // Arrange
            var item = ItemMother.Initial().Create();

            // Act
            item.Delete();

            // Assert
            item.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public void Delete_WithNotDeletedItem_ShouldPublishEvent()
        {
            // Arrange
            var item = ItemMother.Initial().Create();

            // Act
            item.Delete();

            // Assert
            var aggregateRootItem = (AggregateRoot)item;
            aggregateRootItem.DomainEvents.Should().HaveCount(1);
            aggregateRootItem.DomainEvents.First().Should().BeEquivalentTo(new ItemDeletedDomainEvent { ItemId = item.Id });
        }

        [Fact]
        public void Delete_WithDeletedItem_ShouldNotPublishEvent()
        {
            // Arrange
            var item = ItemMother.Deleted().Create();

            // Act
            item.Delete();

            // Assert
            var aggregateRootItem = (AggregateRoot)item;
            aggregateRootItem.DomainEvents.Should().BeEmpty();
        }
    }

    public class IsAvailableInStore
    {
        private readonly CommonFixture _commonFixture = new();

        [Fact]
        public void IsAvailableInStore_WithNotAvailableInStore_ShouldReturnFalse()
        {
            // Arrange
            IItem testObject = ItemMother.Initial().Create();

            // Act
            StoreId storeId = new StoreIdBuilder().Create();
            var result = testObject.IsAvailableInStore(storeId);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeFalse();
            }
        }

        [Fact]
        public void IsAvailableInStore_WithAvailableInStore_ShouldReturnTrue()
        {
            // Arrange
            IItem testObject = ItemMother.Initial().Create();
            var availabilityStoreIds = testObject.Availabilities.Select(av => av.StoreId).ToList();

            // Act
            StoreId chosenStoreId = _commonFixture.ChooseRandom(availabilityStoreIds);
            var result = testObject.IsAvailableInStore(chosenStoreId);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
            }
        }
    }

    public class MakePermanent
    {
        [Fact]
        public void MakePermanent_WithValidData_ShouldMakeItemPermanent()
        {
            // Arrange
            IItem sut = ItemMother.Initial().Create();
            PermanentItem permanentItem = new DomainTestBuilder<PermanentItem>().Create();
            IEnumerable<ItemAvailability> availabilities =
                ItemAvailabilityMother.Initial().CreateMany(3).ToList();

            // Act
            sut.MakePermanent(permanentItem, availabilities);

            // Assert
            using (new AssertionScope())
            {
                sut.Name.Should().Be(permanentItem.Name);
                sut.Comment.Should().Be(permanentItem.Comment);
                sut.ItemQuantity.Should().Be(permanentItem.ItemQuantity);
                sut.Availabilities.Should().BeEquivalentTo(availabilities);
                sut.ItemCategoryId.Should().Be(permanentItem.ItemCategoryId);
                sut.ManufacturerId.Should().Be(permanentItem.ManufacturerId);
                sut.IsTemporary.Should().BeFalse();
            }
        }

        [Fact]
        public void MakePermanent_WithDeleted_ShouldThrow()
        {
            // Arrange
            IItem sut = ItemMother.Initial().WithIsDeleted(true).Create();
            PermanentItem permanentItem = new DomainTestBuilder<PermanentItem>().Create();
            IEnumerable<ItemAvailability> availabilities =
                ItemAvailabilityMother.Initial().CreateMany(3).ToList();

            // Act
            var func = () => sut.MakePermanent(permanentItem, availabilities);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.CannotMakeDeletedItemPermanent);
        }
    }

    public class Modify
    {
        private readonly ModifyFixture _fixture = new();

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Modify_WithExistingAvailabilities_ShouldModifyItem(bool isTemporary)
        {
            // Arrange
            _fixture.SetupWithoutItemTypes();
            _fixture.SetupIsTemporary(isTemporary);
            _fixture.SetupModification();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

            // Act
            sut.Modify(_fixture.Modification, sut.Availabilities);

            // Assert
            using (new AssertionScope())
            {
                sut.Name.Should().Be(_fixture.Modification.Name);
                sut.Comment.Should().Be(_fixture.Modification.Comment);
                sut.ItemQuantity.Should().Be(_fixture.Modification.ItemQuantity);
                sut.Availabilities.Should().BeEquivalentTo(sut.Availabilities);
                sut.ItemCategoryId.Should().Be(_fixture.Modification.ItemCategoryId);
                sut.ManufacturerId.Should().Be(_fixture.Modification.ManufacturerId);
                sut.IsTemporary.Should().Be(isTemporary);
                sut.DomainEvents.Should().BeEmpty();
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Modify_WithNewAvailabilities_ShouldModifyItem(bool isTemporary)
        {
            // Arrange
            _fixture.SetupWithoutItemTypes();
            _fixture.SetupIsTemporary(isTemporary);
            _fixture.SetupModification();
            _fixture.SetupAvailabilities();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedEvent(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Availabilities);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedEvent);

            // Act
            sut.Modify(_fixture.Modification, _fixture.Availabilities);

            // Assert
            using (new AssertionScope())
            {
                sut.Name.Should().Be(_fixture.Modification.Name);
                sut.Comment.Should().Be(_fixture.Modification.Comment);
                sut.ItemQuantity.Should().Be(_fixture.Modification.ItemQuantity);
                sut.Availabilities.Should().BeEquivalentTo(_fixture.Availabilities);
                sut.ItemCategoryId.Should().Be(_fixture.Modification.ItemCategoryId);
                sut.ManufacturerId.Should().Be(_fixture.Modification.ManufacturerId);
                sut.IsTemporary.Should().Be(isTemporary);
                sut.DomainEvents.Should().HaveCount(1);
            }

            sut.DomainEvents.First().Should().BeEquivalentTo(_fixture.ExpectedEvent);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Modify_WithTypes_ShouldThrow(bool isTemporary)
        {
            // Arrange
            _fixture.SetupIsTemporary(isTemporary);
            _fixture.SetupModification();
            _fixture.SetupAvailabilities();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Availabilities);

            // Act
            var func = () => sut.Modify(_fixture.Modification, _fixture.Availabilities);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.CannotModifyItemWithTypesAsItem);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Modify_WithEmptyAvailabilities_ShouldThrow(bool isTemporary)
        {
            // Arrange
            _fixture.SetupWithoutItemTypes();
            _fixture.SetupIsTemporary(isTemporary);
            _fixture.SetupModification();
            _fixture.SetupEmptyAvailabilities();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Availabilities);

            // Act
            var func = () => sut.Modify(_fixture.Modification, _fixture.Availabilities);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.CannotModifyItemWithoutAvailabilities);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Modify_WithDeleted_ShouldThrow(bool isTemporary)
        {
            // Arrange
            _fixture.SetupWithoutItemTypes();
            _fixture.SetupDeleted();
            _fixture.SetupIsTemporary(isTemporary);
            _fixture.SetupModification();
            _fixture.SetupAvailabilities();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Availabilities);

            // Act
            var func = () => sut.Modify(_fixture.Modification, _fixture.Availabilities);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.CannotModifyDeletedItem);
        }

        private sealed class ModifyFixture : ItemFixture
        {
            public ItemModification? Modification { get; private set; }
            public IReadOnlyCollection<ItemAvailability>? Availabilities { get; private set; }
            public ItemAvailabilitiesChangedDomainEvent? ExpectedEvent { get; private set; }

            public void SetupModification()
            {
                Modification = new DomainTestBuilder<ItemModification>().Create();
            }

            public void SetupAvailabilities()
            {
                Availabilities = ItemAvailabilityMother.Initial().CreateMany(3).ToList();
            }

            public void SetupEmptyAvailabilities()
            {
                Availabilities = new List<ItemAvailability>(0);
            }

            public void SetupExpectedEvent(Item sut)
            {
                TestPropertyNotSetException.ThrowIfNull(Availabilities);

                ExpectedEvent = new ItemAvailabilitiesChangedDomainEvent(null, sut.Availabilities, Availabilities)
                {
                    ItemId = sut.Id
                };
            }
        }
    }

    public class ModifyAsync
    {
        private readonly ModifyAsyncFixture _fixture = new();

        [Fact]
        public async Task ModifyAsync_WithOneNewType_ShouldRemoveOldType()
        {
            // Arrange
            _fixture.SetupOneExistingType();
            _fixture.SetupModificationWithOneDifferentType();
            _fixture.SetupValidationSuccess();
            _fixture.SetupFactoryCreatingType();

            var sut = _fixture.CreateSut();
            var existing = sut.ItemTypes.First().DeepClone();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewType);

            // Act
            await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            sut.ItemTypes.Should().HaveCount(2);
            sut.ItemTypes.Should().Contain(_fixture.NewType);

            var existingResult = sut.ItemTypes.FirstOrDefault(t => t.Id == existing.Id);
            existingResult.Should().NotBeNull();
            existingResult.Should().BeEquivalentTo(existing, opt => opt.Excluding(info => info.Path == "IsDeleted"));
            existingResult!.IsDeleted.Should().BeTrue();

            sut.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public async Task ModifyAsync_WithOneExistingType_WithDifferentAvailabilities_ShouldModifyExistingType()
        {
            // Arrange
            _fixture.SetupOneExistingType();
            _fixture.SetupModificationWithOneExistingType();
            _fixture.SetupValidationSuccess();

            var sut = _fixture.CreateSut();
            var existingType = sut.ItemTypes.First();
            _fixture.SetupExpectedModifiedType(existingType);
            _fixture.SetupExpectedEvent(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedModifiedType);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedEvent);

            // Act
            await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            sut.ItemTypes.Should().HaveCount(1);
            sut.ItemTypes.First().Should().BeEquivalentTo(_fixture.ExpectedModifiedType);

            sut.DomainEvents.Should().HaveCount(1);
            sut.DomainEvents.First().Should().BeEquivalentTo(_fixture.ExpectedEvent);
        }

        [Fact]
        public async Task ModifyAsync_WithOneExistingType_WithSameAvailabilities_ShouldModifyExistingType()
        {
            // Arrange
            _fixture.SetupOneExistingType();
            var sut = _fixture.CreateSut();
            var existingType = sut.ItemTypes.First();

            _fixture.SetupModificationWithOneExistingTypeAndSameAvailabilities(existingType);
            _fixture.SetupValidationSuccess();
            _fixture.SetupExpectedModifiedType(existingType);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedModifiedType);

            // Act
            await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            sut.ItemTypes.Should().HaveCount(1);
            sut.ItemTypes.First().Should().BeEquivalentTo(_fixture.ExpectedModifiedType);

            sut.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public async Task ModifyAsync_WithEmptyTypes_ShouldThrow()
        {
            // Arrange
            _fixture.SetupOneExistingType();
            _fixture.SetupModificationWithEmptyTypes();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

            // Act
            var func = async () => await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.CannotRemoveAllTypesFromItemWithTypes);
        }

        [Fact]
        public async Task ModifyAsync_WithEmptyTypeAvailabilities_ShouldThrow()
        {
            // Arrange
            _fixture.SetupOneExistingType();
            _fixture.SetupModificationWithOneExistingTypeAndEmptyAvailabilities();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

            // Act
            var func = async () => await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.CannotModifyItemTypeWithoutAvailabilities);
        }

        [Fact]
        public async Task ModifyAsync_WithModifyingDeletedType_ShouldThrow()
        {
            // Arrange
            _fixture.SetupOneExistingDeletedType();
            _fixture.SetupModificationWithOneExistingType();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

            // Act
            var func = async () => await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.CannotModifyDeletedItemType);
        }

        [Fact]
        public async Task ModifyAsync_WithDeleted_ShouldThrow()
        {
            // Arrange
            _fixture.SetupDeleted();
            _fixture.SetupModification();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

            // Act
            var func = async () => await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.CannotModifyDeletedItem);
        }

        [Fact]
        public async Task ModifyAsync_WithoutItemTypes_ShouldThrow()
        {
            // Arrange
            _fixture.SetupWithoutItemTypes();
            _fixture.SetupModification();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

            // Act
            var func = async () => await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.CannotModifyItemAsItemWithTypes);
        }

        private class ModifyAsyncFixture : ItemFixture
        {
            public ValidatorMock ValidatorMock { get; } = new(MockBehavior.Strict);
            public ItemWithTypesModification? Modification { get; private set; }
            public ItemType? NewType { get; private set; }
            public ItemTypeId? ExistingTypeId { get; private set; }
            public ItemAvailabilitiesChangedDomainEvent? ExpectedEvent { get; private set; }

            public void SetupModification()
            {
                Modification = new DomainTestBuilder<ItemWithTypesModification>().Create();
            }

            public void SetupModificationWithOneDifferentType()
            {
                var types = new DomainTestBuilder<ItemTypeModification>()
                    .FillConstructorWith<ItemTypeId?>("id", null)
                    .CreateMany(1)
                    .ToList();

                Modification = new DomainTestBuilder<ItemWithTypesModification>()
                    .FillConstructorWith("itemTypes", types.AsEnumerable()).Create();
            }

            public void SetupModificationWithOneExistingType()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingTypeId);

                var types = new DomainTestBuilder<ItemTypeModification>()
                    .FillConstructorWith<ItemTypeId?>("id", ExistingTypeId.Value)
                    .CreateMany(1)
                    .ToList();

                Modification = new DomainTestBuilder<ItemWithTypesModification>()
                    .FillConstructorWith("itemTypes", types.AsEnumerable()).Create();
            }

            public void SetupModificationWithOneExistingTypeAndSameAvailabilities(IItemType existingType)
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingTypeId);

                var types = new DomainTestBuilder<ItemTypeModification>()
                    .FillConstructorWith<ItemTypeId?>("id", ExistingTypeId.Value)
                    .FillConstructorWith("availabilities", existingType.Availabilities.AsEnumerable())
                    .CreateMany(1)
                    .ToList();

                Modification = new DomainTestBuilder<ItemWithTypesModification>()
                    .FillConstructorWith("itemTypes", types.AsEnumerable()).Create();
            }

            public void SetupModificationWithOneExistingTypeAndEmptyAvailabilities()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingTypeId);

                var types = new DomainTestBuilder<ItemTypeModification>()
                    .FillConstructorWith<ItemTypeId?>("id", ExistingTypeId.Value)
                    .FillConstructorWith("availabilities", Enumerable.Empty<ItemAvailability>())
                    .CreateMany(1)
                    .ToList();

                Modification = new DomainTestBuilder<ItemWithTypesModification>()
                    .FillConstructorWith("itemTypes", types.AsEnumerable()).Create();
            }

            public void SetupModificationWithEmptyTypes()
            {
                Modification = new DomainTestBuilder<ItemWithTypesModification>()
                    .FillConstructorWith("itemTypes", Enumerable.Empty<ItemTypeModification>()).Create();
            }

            public void SetupOneExistingType()
            {
                ExistingTypeId = ItemTypeId.New;
                var types = ItemTypeMother.Initial().WithId(ExistingTypeId.Value).CreateMany(1).ToList();
                Builder.WithTypes(new ItemTypes(types, ItemTypeFactoryMock.Object));
            }

            public void SetupOneExistingDeletedType()
            {
                ExistingTypeId = ItemTypeId.New;
                var types = ItemTypeMother.Initial().WithIsDeleted(true).WithId(ExistingTypeId.Value).CreateMany(1).ToList();
                Builder.WithTypes(new ItemTypes(types, ItemTypeFactoryMock.Object));
            }

            public void SetupValidationSuccess()
            {
                TestPropertyNotSetException.ThrowIfNull(Modification);

                foreach (var type in Modification.ItemTypes)
                {
                    ValidatorMock.SetupValidateAsync(type.Availabilities);
                }
            }

            public void SetupExpectedModifiedType(IItemType existingType)
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingTypeId);
                TestPropertyNotSetException.ThrowIfNull(Modification);

                var typeModification = Modification.ItemTypes.First();
                ExpectedModifiedType = new ItemType(ExistingTypeId.Value, typeModification.Name,
                    typeModification.Availabilities, existingType.PredecessorId, false);
            }

            public void SetupExpectedEvent(IItem sut)
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingTypeId);
                TestPropertyNotSetException.ThrowIfNull(Modification);

                ExpectedEvent = new ItemAvailabilitiesChangedDomainEvent(ExistingTypeId.Value,
                    sut.ItemTypes.First().Availabilities, Modification.ItemTypes.First().Availabilities)
                {
                    ItemId = sut.Id
                };
            }

            public ItemType? ExpectedModifiedType { get; private set; }

            public void SetupFactoryCreatingType()
            {
                TestPropertyNotSetException.ThrowIfNull(Modification);

                var modificationType = Modification.ItemTypes.First();
                NewType = ItemTypeMother.Initial().Create();
                ItemTypeFactoryMock.SetupCreateNew(modificationType.Name, modificationType.Availabilities, NewType);
            }
        }
    }

    public class GetDefaultSectionIdForStore
    {
        private readonly CommonFixture _commonFixture = new();

        [Fact]
        public void GetDefaultSectionIdForStore_WithInvalidStoreId_ShouldThrowDomainException()
        {
            // Arrange
            IItem testObject = ItemMother.Initial().Create();
            var requestStoreId = new StoreIdBuilder().Create();

            // Act
            Action action = () => testObject.GetDefaultSectionIdForStore(requestStoreId);

            // Assert
            using (new AssertionScope())
            {
                action.Should().Throw<DomainException>()
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemAtStoreNotAvailable);
            }
        }

        [Fact]
        public void GetDefaultSectionIdForStore_WithValidStoreId_ShouldReturnSectionId()
        {
            // Arrange
            IItem testObject = ItemMother.Initial().Create();
            var chosenAvailability = _commonFixture.ChooseRandom(testObject.Availabilities);

            // Act
            var result = testObject.GetDefaultSectionIdForStore(chosenAvailability.StoreId);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(chosenAvailability.DefaultSectionId);
            }
        }
    }

    public class UpdateAsync_WithTypes
    {
        private readonly UpdateAsyncFixture _fixture = new();

        [Fact]
        public async Task UpdateAsync_WithDeleted_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupItemWithTypesUpdateNotCustomized();
            _fixture.SetupDeleted();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemWithTypesUpdate);

            // Act
            var func = async () => await sut.UpdateAsync(_fixture.ItemWithTypesUpdate, _fixture.ValidatorMock.Object,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.CannotUpdateDeletedItem);
        }

        [Fact]
        public async Task UpdateAsync_WithoutTypes_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupOldItemWithoutTypes();
            _fixture.SetupItemWithTypesUpdateNotCustomized();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemWithTypesUpdate);

            // Act
            var func = async () => await sut.UpdateAsync(_fixture.ItemWithTypesUpdate, _fixture.ValidatorMock.Object,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.CannotUpdateItemAsItemWithTypes);
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupOldItem();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedItem(sut);
            _fixture.SetupItemWithTypesUpdate();
            _fixture.SetupUpdatingItemType();
            _fixture.SetupValidatingManufacturerSuccess();
            _fixture.SetupValidatingItemCategorySuccess();
            _fixture.SetupValidatingAvailabilitiesSuccess();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemWithTypesUpdate);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await sut.UpdateAsync(_fixture.ItemWithTypesUpdate, _fixture.ValidatorMock.Object,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt.Excluding(info => info.Path == "Id"));
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldDeleteOldItem()
        {
            // Arrange
            _fixture.SetupOldItem();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedItem(sut);
            _fixture.SetupItemWithTypesUpdate();
            _fixture.SetupUpdatingItemType();
            _fixture.SetupValidatingManufacturerSuccess();
            _fixture.SetupValidatingItemCategorySuccess();
            _fixture.SetupValidatingAvailabilitiesSuccess();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemWithTypesUpdate);

            // Act
            await sut.UpdateAsync(_fixture.ItemWithTypesUpdate, _fixture.ValidatorMock.Object,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            sut.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldSetUpdateOnAtOldItem()
        {
            // Arrange
            _fixture.SetupOldItem();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedItem(sut);
            _fixture.SetupItemWithTypesUpdate();
            _fixture.SetupUpdatingItemType();
            _fixture.SetupValidatingManufacturerSuccess();
            _fixture.SetupValidatingItemCategorySuccess();
            _fixture.SetupValidatingAvailabilitiesSuccess();
            _fixture.SetupDateTimeServiceReturningExpectedUpdatedOn();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemWithTypesUpdate);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedUpdatedOn);

            // Act
            await sut.UpdateAsync(_fixture.ItemWithTypesUpdate, _fixture.ValidatorMock.Object,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            sut.UpdatedOn.Should().Be(_fixture.ExpectedUpdatedOn);
        }

        private sealed class UpdateAsyncFixture : UpdateFixture
        {
            private readonly ItemTypeFactoryMock _itemTypeFactoryMock = new(MockBehavior.Strict);
            private ItemTypeMock? _existingItemTypeMock;
            public ItemWithTypesUpdate? ItemWithTypesUpdate { get; private set; }
            public IItemType? ExpectedItemType { get; private set; }

            public void SetupOldItem()
            {
                _existingItemTypeMock = new ItemTypeMock(
                    new ItemTypeBuilder().WithIsDeleted(false).Create(),
                    MockBehavior.Strict);

                var types = new ItemTypes(_existingItemTypeMock.Object.ToMonoList(), _itemTypeFactoryMock.Object);
                ItemMother.InitialWithTypes(Builder)
                    .WithTypes(types);
            }

            public void SetupOldItemWithoutTypes()
            {
                ItemMother.Initial(Builder);
            }

            public void SetupExpectedItem(IItem sut)
            {
                ExpectedItemType = new ItemTypeBuilder().WithIsDeleted(false).Create();
                ExpectedResult = ItemMother.InitialWithTypes()
                    .WithTypes(new ItemTypes(ExpectedItemType.ToMonoList(), _itemTypeFactoryMock.Object))
                    .WithPredecessorId(sut.Id)
                    .Create();
            }

            public void SetupUpdatingItemType()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingItemTypeMock);
                TestPropertyNotSetException.ThrowIfNull(ItemWithTypesUpdate);
                TestPropertyNotSetException.ThrowIfNull(ExpectedItemType);

                _existingItemTypeMock.SetupUpdateAsync(ItemWithTypesUpdate.TypeUpdates.First(), ValidatorMock.Object,
                    ExpectedItemType);
            }

            public void SetupItemWithTypesUpdate()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult.ItemCategoryId);
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult.ManufacturerId);
                TestPropertyNotSetException.ThrowIfNull(_existingItemTypeMock);

                ItemWithTypesUpdate = new ItemWithTypesUpdate(
                    ItemId.New,
                    ExpectedResult.Name,
                    ExpectedResult.Comment,
                    ExpectedResult.ItemQuantity,
                    ExpectedResult.ItemCategoryId.Value,
                    ExpectedResult.ManufacturerId.Value,
                    ExpectedResult.ItemTypes
                        .Select(t => new ItemTypeUpdate(
                            _existingItemTypeMock.Object.Id,
                            t.Name,
                            t.Availabilities))
                        .ToArray());
            }

            public void SetupItemWithTypesUpdateNotCustomized()
            {
                ItemWithTypesUpdate = new DomainTestBuilder<ItemWithTypesUpdate>().Create();
            }

            public void SetupValidatingItemCategorySuccess()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemWithTypesUpdate);

                SetupValidatingItemCategorySuccess(ItemWithTypesUpdate.ItemCategoryId);
            }

            public void SetupValidatingManufacturerSuccess()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemWithTypesUpdate);
                TestPropertyNotSetException.ThrowIfNull(ItemWithTypesUpdate.ManufacturerId);

                SetupValidatingManufacturerSuccess(ItemWithTypesUpdate.ManufacturerId.Value);
            }

            public void SetupValidatingAvailabilitiesSuccess()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemWithTypesUpdate);

                foreach (var itemTypeUpdate in ItemWithTypesUpdate.TypeUpdates)
                {
                    SetupValidatingAvailabilitiesSuccess(itemTypeUpdate.Availabilities);
                }
            }
        }
    }

    public class UpdateAsync_WithoutTypes
    {
        private readonly UpdateAsyncFixture _fixture = new();

        [Fact]
        public async Task UpdateAsync_WithDeleted_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupItemUpdateNotCustomized();
            _fixture.SetupDeleted();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemUpdate);

            // Act
            var func = async () => await sut.UpdateAsync(_fixture.ItemUpdate, _fixture.ValidatorMock.Object,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.CannotUpdateDeletedItem);
        }

        [Fact]
        public async Task UpdateAsync_WithTypes_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupOldItemWithTypes();
            _fixture.SetupItemUpdateNotCustomized();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemUpdate);

            // Act
            var func = async () => await sut.UpdateAsync(_fixture.ItemUpdate, _fixture.ValidatorMock.Object,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.CannotUpdateItemWithTypesAsItem);
        }

        [Fact]
        public async Task UpdateAsync_WithItemIsTemporary_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupOldItemTemporary();
            _fixture.SetupItemUpdateNotCustomized();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemUpdate);

            // Act
            var func = async () => await sut.UpdateAsync(_fixture.ItemUpdate, _fixture.ValidatorMock.Object,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.TemporaryItemNotUpdateable);
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupOldItem();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedItem(sut);
            _fixture.SetupItemUpdate();
            _fixture.SetupValidatingItemCategorySuccess();
            _fixture.SetupValidatingManufacturerSuccess();
            _fixture.SetupValidatingAvailabilitiesSuccess();
            _fixture.SetupDateTimeServiceReturningExpectedUpdatedOn();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemUpdate);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await sut.UpdateAsync(_fixture.ItemUpdate, _fixture.ValidatorMock.Object,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt.Excluding(info => info.Path == "Id"));
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldDeleteOldItem()
        {
            // Arrange
            _fixture.SetupOldItem();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedItem(sut);
            _fixture.SetupItemUpdate();
            _fixture.SetupValidatingItemCategorySuccess();
            _fixture.SetupValidatingManufacturerSuccess();
            _fixture.SetupValidatingAvailabilitiesSuccess();
            _fixture.SetupDateTimeServiceReturningExpectedUpdatedOn();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemUpdate);

            // Act
            await sut.UpdateAsync(_fixture.ItemUpdate, _fixture.ValidatorMock.Object,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            sut.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldSetUpdateOnAtOldItem()
        {
            // Arrange
            _fixture.SetupOldItem();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedItem(sut);
            _fixture.SetupItemUpdate();
            _fixture.SetupValidatingItemCategorySuccess();
            _fixture.SetupValidatingManufacturerSuccess();
            _fixture.SetupValidatingAvailabilitiesSuccess();
            _fixture.SetupDateTimeServiceReturningExpectedUpdatedOn();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemUpdate);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedUpdatedOn);

            // Act
            await sut.UpdateAsync(_fixture.ItemUpdate, _fixture.ValidatorMock.Object,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            sut.UpdatedOn.Should().Be(_fixture.ExpectedUpdatedOn);
        }

        private sealed class UpdateAsyncFixture : UpdateFixture
        {
            public ItemUpdate? ItemUpdate { get; private set; }

            public void SetupOldItem()
            {
                ItemMother.Initial(Builder);
            }

            public void SetupOldItemWithTypes()
            {
                ItemMother.InitialWithTypes(Builder);
            }

            public void SetupOldItemTemporary()
            {
                ItemMother.InitialTemporary(Builder);
            }

            public void SetupExpectedItem(IItem sut)
            {
                ExpectedResult = ItemMother.Initial().WithPredecessorId(sut.Id).Create();
            }

            public void SetupItemUpdate()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult.ItemCategoryId);
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult.ManufacturerId);

                ItemUpdate = new ItemUpdate(
                    ItemId.New,
                    ExpectedResult.Name,
                    ExpectedResult.Comment,
                    ExpectedResult.ItemQuantity,
                    ExpectedResult.ItemCategoryId.Value,
                    ExpectedResult.ManufacturerId.Value,
                    ExpectedResult.Availabilities);
            }

            public void SetupItemUpdateNotCustomized()
            {
                ItemUpdate = new DomainTestBuilder<ItemUpdate>().Create();
            }

            public void SetupValidatingItemCategorySuccess()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemUpdate);

                SetupValidatingItemCategorySuccess(ItemUpdate.ItemCategoryId);
            }

            public void SetupValidatingManufacturerSuccess()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemUpdate);
                TestPropertyNotSetException.ThrowIfNull(ItemUpdate.ManufacturerId);

                SetupValidatingManufacturerSuccess(ItemUpdate.ManufacturerId.Value);
            }

            public void SetupValidatingAvailabilitiesSuccess()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemUpdate);

                SetupValidatingAvailabilitiesSuccess(ItemUpdate.Availabilities);
            }
        }
    }

    public class Update_Price
    {
        private readonly UpdateFixture_Price _fixture = new();

        [Fact]
        public void Update_WithDeleted_ShouldThrow()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupPrice();
            _fixture.SetupDeleted();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var func = () => sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.CannotUpdateDeletedItem);
        }

        [Fact]
        public void Update_WithItemWithoutTypes_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupAsItemWithoutTypes();
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupPrice();
            _fixture.SetupItemAvailableAtStore();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);
            _fixture.SetupDateTimeServiceReturningExpectedUpdatedOn();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var result = sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.Excluding(info => info.Path == "Id"));
            result.Id.Should().NotBe(sut.Id);
        }

        [Fact]
        public void Update_WithItemWithoutTypes_ShouldDeleteItem()
        {
            // Arrange
            _fixture.SetupAsItemWithoutTypes();
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupPrice();
            _fixture.SetupItemAvailableAtStore();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);
            _fixture.SetupDateTimeServiceReturningExpectedUpdatedOn();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            sut.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public void Update_WithItemWithoutTypes_ShouldSetUpdatedOn()
        {
            // Arrange
            _fixture.SetupAsItemWithoutTypes();
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupPrice();
            _fixture.SetupItemAvailableAtStore();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);
            _fixture.SetupDateTimeServiceReturningExpectedUpdatedOn();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedUpdatedOn);

            // Act
            sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            sut.UpdatedOn.Should().NotBeNull();
            sut.UpdatedOn.Should().Be(_fixture.ExpectedUpdatedOn.Value);
        }

        [Fact]
        public void Update_WithItemWithoutTypesAndInStoreNotAvailable_ShouldThrow()
        {
            // Arrange
            _fixture.SetupAsItemWithoutTypes();
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupPrice();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var func = () => sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.ItemAtStoreNotAvailable);
        }

        [Fact]
        public void Update_WithItemWithTypesAndItemTypeIdNull_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupAsItemWithTypes();
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupPrice();
            _fixture.SetupItemTypeAvailableAtStore();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultWithAllTypesUpdated(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var result = sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.Excluding(info =>
                    info.Path == "Id"
                    || Regex.IsMatch(info.Path, @"ItemTypes\[\d+\].Id")));

            result.Id.Should().NotBe(sut.Id);
            var newItemTypeIds = result.ItemTypes.Select(t => t.Id).ToList();
            foreach (var existingItemTypeId in sut.ItemTypes.Select(t => t.Id))
            {
                newItemTypeIds.Should().NotContain(existingItemTypeId);
            }
        }

        [Fact]
        public void Update_WithItemWithTypesAndItemTypeIdNotNull_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupAsItemWithTypes();
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeId();
            _fixture.SetupPrice();
            _fixture.SetupItemTypeAvailableAtStore();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultWithOneTypeUpdated(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var result = sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.Excluding(info =>
                    info.Path == "Id"
                    || Regex.IsMatch(info.Path, @"ItemTypes\[\d+\].Id")));

            result.Id.Should().NotBe(sut.Id);
            var newItemTypeIds = result.ItemTypes.Select(t => t.Id).ToList();
            foreach (var existingItemTypeId in sut.ItemTypes.Select(t => t.Id))
            {
                newItemTypeIds.Should().NotContain(existingItemTypeId);
            }
        }

        private sealed class UpdateFixture_Price : UpdateFixture
        {
            private readonly ItemTypeFactoryMock _itemTypeFactoryMock = new(MockBehavior.Strict);

            public Price? Price { get; private set; }
            public StoreId? StoreId { get; private set; }
            public ItemTypeId? ItemTypeId { get; private set; }

            public void SetupAsItemWithTypes()
            {
                ItemMother.InitialWithTypes(Builder);
            }

            public void SetupAsItemWithoutTypes()
            {
                ItemMother.Initial(Builder);
            }

            public void SetupItemAvailableAtStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);
                Builder.WithAvailability(ItemAvailabilityMother.ForStore(StoreId.Value).Create());
            }

            public void SetupItemTypeAvailableAtStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);
                var type1 = new ItemTypeBuilder()
                    .WithAvailability(ItemAvailabilityMother.ForStore(StoreId.Value).Create())
                    .WithId(ItemTypeId ?? Domain.Items.Models.ItemTypeId.New)
                    .WithIsDeleted(false)
                    .Create();
                var type2 = new ItemTypeBuilder()
                    .WithIsDeleted(false)
                    .Create();

                Builder.WithTypes(new ItemTypes(new List<IItemType> { type1, type2 }, _itemTypeFactoryMock.Object));
            }

            public void SetupStoreId()
            {
                StoreId = Domain.Stores.Models.StoreId.New;
            }

            public void SetupItemTypeId()
            {
                ItemTypeId = Domain.Items.Models.ItemTypeId.New;
            }

            public void SetupItemTypeIdNull()
            {
                ItemTypeId = null;
            }

            public void SetupPrice()
            {
                Price = new DomainTestBuilder<Price>().Create();
            }

            public void SetupExpectedResult(Item item)
            {
                TestPropertyNotSetException.ThrowIfNull(Price);

                ExpectedResult = new Item(
                    ItemId.New,
                    item.Name,
                    false,
                    item.Comment,
                    item.IsTemporary,
                    item.ItemQuantity,
                    item.ItemCategoryId,
                    item.ManufacturerId,
                    item.Availabilities.Select(av => new ItemAvailability(av.StoreId, Price.Value, av.DefaultSectionId)),
                    item.TemporaryId,
                    null,
                    item.Id);
            }

            public void SetupExpectedResultWithAllTypesUpdated(Item item)
            {
                TestPropertyNotSetException.ThrowIfNull(Price);
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                ExpectedResult = new Item(
                    ItemId.New,
                    item.Name,
                    false,
                    item.Comment,
                    item.ItemQuantity,
                    item.ItemCategoryId!.Value,
                    item.ManufacturerId,
                    new ItemTypes(item.ItemTypes.Select(t =>
                        {
                            var type = new ItemType(
                                Domain.Items.Models.ItemTypeId.New,
                                t.Name,
                                t.Availabilities.Select(av => av.StoreId == StoreId
                                    ? new ItemAvailability(StoreId.Value, Price.Value, av.DefaultSectionId)
                                    : av),
                                t.Id,
                                t.IsDeleted);
                            return type;
                        }),
                        _itemTypeFactoryMock.Object),
                    null,
                    item.Id);
            }

            public void SetupExpectedResultWithOneTypeUpdated(Item item)
            {
                TestPropertyNotSetException.ThrowIfNull(Price);
                TestPropertyNotSetException.ThrowIfNull(StoreId);
                TestPropertyNotSetException.ThrowIfNull(ItemTypeId);

                ExpectedResult = new Item(
                    ItemId.New,
                    item.Name,
                    false,
                    item.Comment,
                    item.ItemQuantity,
                    item.ItemCategoryId!.Value,
                    item.ManufacturerId,
                    new ItemTypes(item.ItemTypes.Select(t =>
                        {
                            var type = new ItemType(
                                Domain.Items.Models.ItemTypeId.New,
                                t.Name,
                                t.Availabilities.Select(av => t.Id == ItemTypeId.Value
                                    ? new ItemAvailability(StoreId.Value, Price.Value, av.DefaultSectionId)
                                    : av),
                                t.Id,
                                t.IsDeleted);
                            return type;
                        }),
                        _itemTypeFactoryMock.Object),
                    null,
                    item.Id);
            }
        }
    }

    private abstract class UpdateFixture : ItemFixture
    {
        public ValidatorMock ValidatorMock { get; } = new(MockBehavior.Strict);
        public DateTimeServiceMock DateTimeServiceMock { get; } = new(MockBehavior.Strict);
        public Item? ExpectedResult { get; protected set; }
        public DateTimeOffset? ExpectedUpdatedOn { get; private set; }

        protected void SetupValidatingItemCategorySuccess(ItemCategoryId itemCategoryId)
        {
            ValidatorMock.SetupValidateAsync(itemCategoryId);
        }

        protected void SetupValidatingManufacturerSuccess(ManufacturerId manufacturerId)
        {
            ValidatorMock.SetupValidateAsync(manufacturerId);
        }

        protected void SetupValidatingAvailabilitiesSuccess(IEnumerable<ItemAvailability> availabilities)
        {
            ValidatorMock.SetupValidateAsync(availabilities);
        }

        public void SetupDateTimeServiceReturningExpectedUpdatedOn()
        {
            ExpectedUpdatedOn = DateTimeOffset.UtcNow;
            DateTimeServiceMock.SetupUtcNow(ExpectedUpdatedOn.Value);
        }
    }

    public class TransferToDefaultSection
    {
        private readonly TransferToDefaultSectionFixture _fixture = new();

        [Fact]
        public void TransferToDefaultSection_WithDeleted_ShouldThrow()
        {
            // Arrange
            _fixture.SetupSectionIds();
            _fixture.SetupDeleted();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewSectionId);

            // Act
            var func = () => sut.TransferToDefaultSection(_fixture.OldSectionId.Value, _fixture.NewSectionId.Value);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.CannotTransferDeletedItem);
        }

        [Fact]
        public void TransferToDefaultSection_WithAvailabilityInOldSection_ShouldTransferToNewSection()
        {
            // Arrange
            _fixture.SetupWithoutTypes();
            _fixture.SetupSectionIds();
            _fixture.SetupAvailabilityInOldSection();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewSectionId);

            // Act
            sut.TransferToDefaultSection(_fixture.OldSectionId.Value, _fixture.NewSectionId.Value);

            // Assert
            sut.Availabilities.Single().DefaultSectionId.Should().Be(_fixture.NewSectionId.Value);
        }

        [Fact]
        public void TransferToDefaultSection_WithAvailabilityNotInOldSection_ShouldNotTransferToNewSection()
        {
            // Arrange
            _fixture.SetupWithoutTypes();
            _fixture.SetupSectionIds();
            _fixture.SetupAvailabilityNotInOldSection();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewSectionId);

            // Act
            sut.TransferToDefaultSection(_fixture.OldSectionId.Value, _fixture.NewSectionId.Value);

            // Assert
            sut.Availabilities.Single().DefaultSectionId.Should().NotBe(_fixture.NewSectionId.Value);
        }

        [Fact]
        public void TransferToDefaultSection_WithTypeAvailabilityInOldSection_ShouldNotTransferToNewSection()
        {
            // Arrange
            _fixture.SetupWithTypes();
            _fixture.SetupSectionIds();
            _fixture.SetupTypeAvailabilityInOldSection();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewSectionId);

            // Act
            sut.TransferToDefaultSection(_fixture.OldSectionId.Value, _fixture.NewSectionId.Value);

            // Assert
            sut.ItemTypes.Single().Availabilities.Single().DefaultSectionId.Should().Be(_fixture.NewSectionId.Value);
        }

        [Fact]
        public void TransferToDefaultSection_WithTypeAvailabilityNotInOldSection_ShouldNotTransferToNewSection()
        {
            // Arrange
            _fixture.SetupWithTypes();
            _fixture.SetupSectionIds();
            _fixture.SetupTypeAvailabilityNotInOldSection();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewSectionId);

            // Act
            sut.TransferToDefaultSection(_fixture.OldSectionId.Value, _fixture.NewSectionId.Value);

            // Assert
            sut.ItemTypes.Single().Availabilities.Single().DefaultSectionId.Should().NotBe(_fixture.NewSectionId.Value);
        }

        private sealed class TransferToDefaultSectionFixture : ItemFixture
        {
            public SectionId? OldSectionId { get; private set; }
            public SectionId? NewSectionId { get; private set; }

            public void SetupWithTypes()
            {
                ItemMother.InitialWithTypes(Builder);
            }

            public void SetupWithoutTypes()
            {
                ItemMother.Initial(Builder);
            }

            public void SetupSectionIds()
            {
                OldSectionId = SectionId.New;
                NewSectionId = SectionId.New;
            }

            public void SetupTypeAvailabilityInOldSection()
            {
                TestPropertyNotSetException.ThrowIfNull(OldSectionId);

                var av = new ItemAvailabilityBuilder()
                    .WithDefaultSectionId(OldSectionId.Value)
                    .Create();

                var type = new ItemTypeBuilder()
                    .WithAvailability(av)
                    .WithIsDeleted(false)
                    .CreateMany(1);

                var types = new ItemTypes(type, ItemTypeFactoryMock.Object);

                Builder.WithTypes(types);
            }

            public void SetupTypeAvailabilityNotInOldSection()
            {
                TestPropertyNotSetException.ThrowIfNull(OldSectionId);

                var av = new ItemAvailabilityBuilder()
                    .Create();

                var type = new ItemTypeBuilder()
                    .WithAvailability(av)
                    .WithIsDeleted(false)
                    .CreateMany(1);

                var types = new ItemTypes(type, ItemTypeFactoryMock.Object);

                Builder.WithTypes(types);
            }

            public void SetupAvailabilityInOldSection()
            {
                TestPropertyNotSetException.ThrowIfNull(OldSectionId);

                var av = new ItemAvailabilityBuilder()
                    .WithDefaultSectionId(OldSectionId.Value)
                    .Create();

                Builder.WithAvailability(av);
            }

            public void SetupAvailabilityNotInOldSection()
            {
                TestPropertyNotSetException.ThrowIfNull(OldSectionId);

                var av = new ItemAvailabilityBuilder()
                    .Create();

                Builder.WithAvailability(av);
            }
        }
    }

    public class GetDefaultSectionIdForStore_ItemType
    {
        private readonly GetDefaultSectionIdForStoreFixture _fixture = new();

        [Fact]
        public void GetDefaultSectionIdForStore_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeId();
            _fixture.SetupExpectedResult();
            _fixture.SetupAvailableAtStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.GetDefaultSectionIdForStore(_fixture.StoreId.Value, _fixture.ItemTypeId.Value);

            // Assert
            result.Should().Be(_fixture.ExpectedResult.Value);
        }

        [Fact]
        public void GetDefaultSectionIdForStore_WithNotAvailableAtStore_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeId();
            _fixture.SetupNotAvailableAtStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);

            // Act
            var func = () => sut.GetDefaultSectionIdForStore(_fixture.StoreId.Value, _fixture.ItemTypeId.Value);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.ItemAtStoreNotAvailable);
        }

        [Fact]
        public void GetDefaultSectionIdForStore_WithNotContainingItemType_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeId();
            _fixture.SetupNotContainingItemType();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);

            // Act
            var func = () => sut.GetDefaultSectionIdForStore(_fixture.StoreId.Value, _fixture.ItemTypeId.Value);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.ItemTypeNotFound);
        }

        [Fact]
        public void GetDefaultSectionIdForStore_WithNotHavingItemTypes_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeId();
            _fixture.SetupNotHavingItemTypes();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);

            // Act
            var func = () => sut.GetDefaultSectionIdForStore(_fixture.StoreId.Value, _fixture.ItemTypeId.Value);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.ItemHasNoItemTypes);
        }

        private sealed class GetDefaultSectionIdForStoreFixture : ItemFixture
        {
            public StoreId? StoreId { get; set; }
            public ItemTypeId? ItemTypeId { get; set; }
            public SectionId? ExpectedResult { get; set; }

            public void SetupStoreId()
            {
                StoreId = Domain.Stores.Models.StoreId.New;
            }

            public void SetupItemTypeId()
            {
                ItemTypeId = Domain.Items.Models.ItemTypeId.New;
            }

            public void SetupExpectedResult()
            {
                ExpectedResult = SectionId.New;
            }

            public void SetupAvailableAtStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);
                TestPropertyNotSetException.ThrowIfNull(ItemTypeId);
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                var availability = new ItemAvailabilityBuilder()
                    .WithStoreId(StoreId.Value)
                    .WithDefaultSectionId(ExpectedResult.Value)
                    .Create();

                var itemTypes = new ItemTypeBuilder()
                    .WithId(ItemTypeId.Value)
                    .WithAvailability(availability)
                    .WithIsDeleted(false)
                    .CreateMany(1);

                Builder.WithTypes(new ItemTypes(itemTypes, ItemTypeFactoryMock.Object));
            }

            public void SetupNotAvailableAtStore()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemTypeId);

                var itemTypes = new ItemTypeBuilder()
                    .WithId(ItemTypeId.Value)
                    .WithIsDeleted(false)
                    .CreateMany(1);

                Builder.WithTypes(new ItemTypes(itemTypes, ItemTypeFactoryMock.Object));
            }

            public void SetupNotContainingItemType()
            {
                var itemTypes = new ItemTypeBuilder()
                    .WithIsDeleted(false)
                    .CreateMany(1);

                Builder.WithTypes(new ItemTypes(itemTypes, ItemTypeFactoryMock.Object));
            }

            public void SetupNotHavingItemTypes()
            {
                Builder.AsItem();
            }
        }
    }

    public class RemoveManufacturer
    {
        private readonly RemoveManufacturerFixture _fixture = new();

        [Fact]
        public void RemoveManufacturer_WithDeleted_ShouldThrow()
        {
            // Arrange
            _fixture.SetupDeleted();
            var sut = _fixture.CreateSut();

            // Act
            var func = () => sut.RemoveManufacturer();

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.CannotRemoveManufacturerFromDeletedItem);
        }

        private class RemoveManufacturerFixture : ItemFixture
        {
        }
    }

    public class RemoveAvailabilitiesFor
    {
        private readonly RemoveAvailabilitiesForFixture _fixture = new();

        [Fact]
        public void RemoveAvailabilitiesFor_WithItemDeleted_WithMultipleAvailabilities_WithOneForStore_ShouldNotChangeItem()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupDeleted();
            _fixture.SetupMultipleWithOneAvailableAtStore();
            var sut = _fixture.CreateSut();
            var expectedItem = sut.DeepClone();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);

            // Act
            sut.RemoveAvailabilitiesFor(_fixture.StoreId.Value);

            // Assert
            sut.Should().BeEquivalentTo(expectedItem);
            sut.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void RemoveAvailabilitiesFor_WithMultipleAvailabilities_WithOneForStore_ShouldDeleteAvailabilityAndPublishEvent()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupMultipleWithOneAvailableAtStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItemAvailabilities);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItemAvailabilityDeletedDomainEvent);

            // Act
            sut.RemoveAvailabilitiesFor(_fixture.StoreId.Value);

            // Assert
            sut.IsDeleted.Should().BeFalse();
            sut.Availabilities.Should().BeEquivalentTo(_fixture.ExpectedItemAvailabilities);

            sut.DomainEvents.Should().HaveCount(1);
            sut.DomainEvents.First().Should().BeOfType<ItemAvailabilityDeletedDomainEvent>();
            var domainEvent = sut.DomainEvents.First() as ItemAvailabilityDeletedDomainEvent;
            domainEvent.Should().BeEquivalentTo(_fixture.ExpectedItemAvailabilityDeletedDomainEvent);
        }

        [Fact]
        public void RemoveAvailabilitiesFor_WithMultipleAvailabilities_WithNoneForStore_ShouldNotChangeItem()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupMultipleWithoutAvailableAtStore();
            var sut = _fixture.CreateSut();
            var expectedItem = sut.DeepClone();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);

            // Act
            sut.RemoveAvailabilitiesFor(_fixture.StoreId.Value);

            // Assert
            sut.Should().BeEquivalentTo(expectedItem);
            sut.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void RemoveAvailabilitiesFor_WithOneAvailabilityForStore_ShouldDeleteItemAndPublishEvent()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupOneAvailableAtStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItemAvailabilities);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItemDeletedDomainEvent);

            // Act
            sut.RemoveAvailabilitiesFor(_fixture.StoreId.Value);

            // Assert
            sut.IsDeleted.Should().BeTrue();
            sut.Availabilities.Should().BeEquivalentTo(_fixture.ExpectedItemAvailabilities);

            sut.DomainEvents.Should().HaveCount(1);
            sut.DomainEvents.First().Should().BeOfType<ItemDeletedDomainEvent>();
            var domainEvent = sut.DomainEvents.First() as ItemDeletedDomainEvent;
            domainEvent.Should().BeEquivalentTo(_fixture.ExpectedItemDeletedDomainEvent);
        }

        [Fact]
        public void RemoveAvailabilitiesFor_WithOneType_WithOneAvailabilityForStore_ShouldDeleteItemAndPublishEvent()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupOneTypeAvailableAtStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItemAvailabilities);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItemDeletedDomainEvent);

            // Act
            sut.RemoveAvailabilitiesFor(_fixture.StoreId.Value);

            // Assert
            sut.IsDeleted.Should().BeTrue();
            var itemType = sut.ItemTypes.First(t => t.Availabilities.Any(av => av.StoreId == _fixture.StoreId.Value));
            itemType.IsDeleted.Should().BeFalse();
            itemType.Availabilities.Should().BeEquivalentTo(_fixture.ExpectedItemAvailabilities);

            sut.DomainEvents.Should().HaveCount(1);
            sut.DomainEvents.First().Should().BeOfType<ItemDeletedDomainEvent>();
            var domainEvent = sut.DomainEvents.First() as ItemDeletedDomainEvent;
            domainEvent.Should().BeEquivalentTo(_fixture.ExpectedItemDeletedDomainEvent);
        }

        [Fact]
        public void RemoveAvailabilitiesFor_WithMultipleTypes_WithOneAvailabilityForStore_ShouldDeleteItemTypeAndPublishEvent()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupMultipleTypesWithOneOnlyAvailableAtStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItemAvailabilities);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItemTypeDeletedDomainEvent);

            // Act
            sut.RemoveAvailabilitiesFor(_fixture.StoreId.Value);

            // Assert
            sut.IsDeleted.Should().BeFalse();
            var itemTypeToDelete = sut.ItemTypes.First(t => t.Availabilities.Any(av => av.StoreId == _fixture.StoreId.Value));
            var itemTypeToNotDelete = sut.ItemTypes.First(t => t.Availabilities.All(av => av.StoreId != _fixture.StoreId.Value));
            itemTypeToDelete.IsDeleted.Should().BeTrue();
            itemTypeToNotDelete.IsDeleted.Should().BeFalse();
            itemTypeToDelete.Availabilities.Should().BeEquivalentTo(_fixture.ExpectedItemAvailabilities);

            sut.DomainEvents.Should().HaveCount(1);
            sut.DomainEvents.First().Should().BeOfType<ItemTypeDeletedDomainEvent>();
            var domainEvent = sut.DomainEvents.First() as ItemTypeDeletedDomainEvent;
            domainEvent.Should().BeEquivalentTo(_fixture.ExpectedItemTypeDeletedDomainEvent);
        }

        [Fact]
        public void RemoveAvailabilitiesFor_WithMultipleTypes_WithoutAvailabilityForStore_ShouldNotChangeItem()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupMultipleTypesWithoutAvailableAtStore();
            var sut = _fixture.CreateSut();
            var expectedItem = sut.DeepClone();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);

            // Act
            sut.RemoveAvailabilitiesFor(_fixture.StoreId.Value);

            // Assert
            sut.Should().BeEquivalentTo(expectedItem);
            sut.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void RemoveAvailabilitiesFor_WithMultipleTypes_WithMultipleAvailabilities_WithOneForStore_ShouldDeleteAvailabilityAndPublishEvent()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupMultipleTypesWithOneNotOnlyAvailableAtStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItemAvailabilities);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItemTypeAvailabilityDeletedDomainEvent);

            // Act
            sut.RemoveAvailabilitiesFor(_fixture.StoreId.Value);

            // Assert
            sut.IsDeleted.Should().BeFalse();
            var itemType = sut.ItemTypes.First();
            itemType.IsDeleted.Should().BeFalse();
            itemType.Availabilities.Should().BeEquivalentTo(_fixture.ExpectedItemAvailabilities);

            sut.DomainEvents.Should().HaveCount(1);
            sut.DomainEvents.First().Should().BeOfType<ItemTypeAvailabilityDeletedDomainEvent>();
            var domainEvent = sut.DomainEvents.First() as ItemTypeAvailabilityDeletedDomainEvent;
            domainEvent.Should().BeEquivalentTo(_fixture.ExpectedItemTypeAvailabilityDeletedDomainEvent);
        }

        private sealed class RemoveAvailabilitiesForFixture : ItemFixture
        {
            public StoreId? StoreId { get; private set; }
            public IReadOnlyCollection<ItemAvailability>? ExpectedItemAvailabilities { get; private set; }
            public ItemDeletedDomainEvent? ExpectedItemDeletedDomainEvent { get; private set; }
            public ItemTypeDeletedDomainEvent? ExpectedItemTypeDeletedDomainEvent { get; private set; }
            public ItemAvailabilityDeletedDomainEvent? ExpectedItemAvailabilityDeletedDomainEvent { get; private set; }
            public ItemTypeAvailabilityDeletedDomainEvent? ExpectedItemTypeAvailabilityDeletedDomainEvent { get; private set; }

            public void SetupStoreId()
            {
                StoreId = Domain.Stores.Models.StoreId.New;
            }

            public void SetupOneTypeAvailableAtStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                var availability = ItemAvailabilityMother.ForStore(StoreId.Value).Create();
                var itemTypes = ItemTypeMother.InitialAvailableAt(availability).CreateMany(1);

                Builder.WithTypes(new ItemTypes(itemTypes, ItemTypeFactoryMock.Object));

                ExpectedItemAvailabilities = new List<ItemAvailability> { availability };
                ExpectedItemDeletedDomainEvent = new ItemDeletedDomainEvent { ItemId = Id };
            }

            public void SetupMultipleTypesWithOneOnlyAvailableAtStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                var availability = ItemAvailabilityMother.ForStore(StoreId.Value).Create();

                var itemTypes = new List<IItemType>{
                    ItemTypeMother.InitialAvailableAt(availability).Create(),
                    ItemTypeMother.Initial().Create(),
                };

                Builder.WithTypes(new ItemTypes(itemTypes, ItemTypeFactoryMock.Object));

                ExpectedItemAvailabilities = new List<ItemAvailability> { availability };
                ExpectedItemTypeDeletedDomainEvent = new ItemTypeDeletedDomainEvent(itemTypes.First().Id) { ItemId = Id };
            }

            public void SetupMultipleTypesWithoutAvailableAtStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                var itemTypes = new List<IItemType>{
                    ItemTypeMother.Initial().Create(),
                    ItemTypeMother.Initial().Create(),
                };

                Builder.WithTypes(new ItemTypes(itemTypes, ItemTypeFactoryMock.Object));
            }

            public void SetupMultipleTypesWithOneNotOnlyAvailableAtStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                var availabilities = new List<ItemAvailability>
                {
                    ItemAvailabilityMother.ForStore(StoreId.Value).Create(),
                    ItemAvailabilityMother.Initial().Create()
                };

                var itemTypes = new List<IItemType>{
                    ItemTypeMother.Initial().WithAvailabilities(availabilities).Create(),
                    ItemTypeMother.Initial().Create(),
                };

                Builder.WithTypes(new ItemTypes(itemTypes, ItemTypeFactoryMock.Object));

                ExpectedItemAvailabilities = new List<ItemAvailability> { itemTypes.First().Availabilities.Last() };
                ExpectedItemTypeAvailabilityDeletedDomainEvent =
                    new ItemTypeAvailabilityDeletedDomainEvent(itemTypes.First().Id, itemTypes.First().Availabilities.First())
                    {
                        ItemId = Id
                    };
            }

            public void SetupOneAvailableAtStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                var availability = ItemAvailabilityMother.ForStore(StoreId.Value).Create();

                Builder.AsItem().WithAvailability(availability);

                ExpectedItemAvailabilities = new List<ItemAvailability> { availability };
                ExpectedItemDeletedDomainEvent = new ItemDeletedDomainEvent { ItemId = Id };
            }

            public void SetupMultipleWithOneAvailableAtStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                var availabilities = new List<ItemAvailability>
                {
                    ItemAvailabilityMother.ForStore(StoreId.Value).Create(),
                    ItemAvailabilityMother.Initial().Create()
                };

                Builder.AsItem().WithAvailabilities(availabilities);

                ExpectedItemAvailabilities = new List<ItemAvailability> { availabilities.Last() };
                ExpectedItemAvailabilityDeletedDomainEvent =
                    new ItemAvailabilityDeletedDomainEvent(availabilities.First()) { ItemId = Id };
            }

            public void SetupMultipleWithoutAvailableAtStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                var availabilities = new List<ItemAvailability>
                {
                    ItemAvailabilityMother.Initial().Create(),
                    ItemAvailabilityMother.Initial().Create()
                };

                Builder.AsItem().WithAvailabilities(availabilities);
            }
        }
    }

    private abstract class ItemFixture
    {
        protected readonly ItemId Id = ItemId.New;

        protected readonly ItemBuilder Builder = new();
        protected readonly ItemTypeFactoryMock ItemTypeFactoryMock = new(MockBehavior.Strict);

        protected ItemFixture()
        {
            Builder.WithIsDeleted(false).WithId(Id);
        }

        public void SetupWithoutItemTypes()
        {
            Builder.AsItem();
        }

        public void SetupDeleted()
        {
            Builder.WithIsDeleted(true);
        }

        public void SetupIsTemporary(bool isTemporary)
        {
            Builder.WithIsTemporary(isTemporary);
        }

        public Item CreateSut()
        {
            return Builder.Create();
        }
    }
}