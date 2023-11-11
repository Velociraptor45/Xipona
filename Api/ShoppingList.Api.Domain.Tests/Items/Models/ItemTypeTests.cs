using Force.DeepCloner;
using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Shared;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.Models;

public class ItemTypeTests
{
    public class Ctor
    {
        private readonly CtorFixture _fixture = new();

        [Fact]
        public void Ctor_WithValidData_ShouldSetProperties()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupName();
            _fixture.SetupAvailabilities();
            _fixture.SetupPredecessorId();
            _fixture.SetupIsDeleted();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Id);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Name);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Availabilities);
            TestPropertyNotSetException.ThrowIfNull(_fixture.PredecessorId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.IsDeleted);

            // Act
            var result = new ItemType(_fixture.Id.Value, _fixture.Name, _fixture.Availabilities,
                _fixture.PredecessorId, _fixture.IsDeleted.Value);

            // Assert
            result.Id.Should().Be(_fixture.Id.Value);
            result.Name.Should().Be(_fixture.Name);
            result.Availabilities.Should().BeEquivalentTo(_fixture.Availabilities);
            result.PredecessorId.Should().Be(_fixture.PredecessorId);
            result.IsDeleted.Should().Be(_fixture.IsDeleted.Value);
        }

        [Fact]
        public void Ctor_WithEmptyAvailabilities_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupName();
            _fixture.SetupEmptyAvailabilities();
            _fixture.SetupPredecessorId();
            _fixture.SetupIsDeleted();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Id);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Name);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Availabilities);
            TestPropertyNotSetException.ThrowIfNull(_fixture.PredecessorId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.IsDeleted);

            // Act
            var func = () => new ItemType(_fixture.Id.Value, _fixture.Name, _fixture.Availabilities,
                _fixture.PredecessorId, _fixture.IsDeleted.Value);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.CannotCreateItemTypeWithoutAvailabilities);
        }

        private sealed class CtorFixture
        {
            public ItemTypeId? Id { get; set; }
            public ItemTypeName? Name { get; private set; }
            public IReadOnlyCollection<ItemAvailability>? Availabilities { get; private set; }
            public ItemTypeId? PredecessorId { get; set; }
            public bool? IsDeleted { get; private set; }

            public void SetupId()
            {
                Id = ItemTypeId.New;
            }

            public void SetupName()
            {
                Name = new DomainTestBuilder<ItemTypeName>().Create();
            }

            public void SetupAvailabilities()
            {
                Availabilities = ItemAvailabilityMother.Initial().CreateMany(3).ToList();
            }

            public void SetupEmptyAvailabilities()
            {
                Availabilities = new List<ItemAvailability>(0);
            }

            public void SetupPredecessorId()
            {
                PredecessorId = ItemTypeId.New;
            }

            public void SetupIsDeleted()
            {
                IsDeleted = new DomainTestBuilder<bool>().Create();
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
            var sut = ItemTypeMother.Initial().Create();
            var requestStoreId = StoreId.New;

            // Act
            var act = () => sut.GetDefaultSectionIdForStore(requestStoreId);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemTypeAtStoreNotAvailable);
        }

        [Fact]
        public void GetDefaultSectionIdForStore_WithValidStoreId_ShouldReturnSectionId()
        {
            // Arrange
            var sut = ItemTypeMother.Initial().Create();
            var chosenAvailability = _commonFixture.ChooseRandom(sut.Availabilities);

            // Act
            var result = sut.GetDefaultSectionIdForStore(chosenAvailability.StoreId);

            // Assert
            result.Should().BeEquivalentTo(chosenAvailability.DefaultSectionId);
        }
    }

    public class IsAvailableAt_Store
    {
        [Fact]
        public void IsAvailableAt_WithInvalidStoreId_ShouldReturnFalse()
        {
            // Arrange
            var sut = ItemTypeMother.Initial().Create();
            var requestStoreId = StoreId.New;

            // Act
            var result = sut.IsAvailableAt(requestStoreId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsAvailableAt_WithValidStoreId_ShouldReturnTrue()
        {
            // Arrange
            var sut = ItemTypeMother.Initial().Create();
            var chosenAvailability = sut.Availabilities.First();

            // Act
            var result = sut.IsAvailableAt(chosenAvailability.StoreId);

            // Assert
            result.Should().BeTrue();
        }
    }

    public class IsAvailableAt_Section
    {
        [Fact]
        public void IsAvailableAt_WithInvalidSectionId_ShouldReturnFalse()
        {
            // Arrange
            var sut = ItemTypeMother.Initial().Create();
            var requestSectionId = SectionId.New;

            // Act
            var result = sut.IsAvailableAt(requestSectionId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsAvailableAt_WithValidSectionId_ShouldReturnTrue()
        {
            // Arrange
            var sut = ItemTypeMother.Initial().Create();
            var chosenAvailability = sut.Availabilities.First();

            // Act
            var result = sut.IsAvailableAt(chosenAvailability.DefaultSectionId);

            // Assert
            result.Should().BeTrue();
        }
    }

    public class ModifyAsync
    {
        private readonly ModifyAsyncFixture _fixture = new();

        [Fact]
        public async Task ModifyAsync_WithValidationSuccess_WithSameAvailabilities_ShouldReturnExpectedResult()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultWithSameAvailabilities(sut);
            _fixture.SetupModificationWithSameAvailabilities(sut);
            _fixture.SetupValidationSuccess();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var (resultItemType, resultDomainEvents) = await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            resultItemType.Should().BeEquivalentTo(_fixture.ExpectedResult);
            resultDomainEvents.Should().BeEmpty();
        }

        [Fact]
        public async Task ModifyAsync_WithValidationSuccess_WithDifferentAvailabilities_ShouldReturnExpectedResult()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);
            _fixture.SetupModification();
            _fixture.SetupValidationSuccess();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var (resultItemType, resultDomainEvents) = await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            resultItemType.Should().BeEquivalentTo(_fixture.ExpectedResult);
            var resultDomainEventsList = resultDomainEvents.ToList();
            resultDomainEventsList.Should().HaveCount(1);
            var resultDomainEvent = resultDomainEventsList.First();
            resultDomainEvent.Should().BeOfType<ItemAvailabilitiesChangedDomainEvent>();
        }

        [Fact]
        public async Task ModifyAsync_WithValidationFailed_ShouldReturnExpectedResult()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);
            _fixture.SetupModification();
            _fixture.SetupValidationFailure();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedException);

            // Act
            var func = async () => await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            await func.Should().ThrowExactlyAsync<InvalidOperationException>()
                .WithMessage(_fixture.ExpectedException.Message);
        }

        private sealed class ModifyAsyncFixture : ItemTypeFixture
        {
            public ValidatorMock ValidatorMock { get; } = new(MockBehavior.Strict);
            public ItemTypeModification? Modification { get; private set; }
            public InvalidOperationException? ExpectedException { get; private set; }
            public ItemType? ExpectedResult { get; private set; }

            public void SetupExpectedResult(IItemType sut)
            {
                ExpectedResult = new ItemTypeBuilder()
                    .WithId(sut.Id)
                    .WithPredecessorId(sut.PredecessorId)
                    .WithIsDeleted(false)
                    .Create();
            }

            public void SetupExpectedResultWithSameAvailabilities(IItemType sut)
            {
                ExpectedResult = new ItemTypeBuilder()
                    .WithId(sut.Id)
                    .WithPredecessorId(sut.PredecessorId)
                    .WithIsDeleted(false)
                    .WithAvailabilities(sut.Availabilities)
                    .Create();
            }

            public void SetupModification()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                Modification = new ItemTypeModification(ExpectedResult.Id, ExpectedResult.Name,
                    ExpectedResult.Availabilities);
            }

            public void SetupModificationWithSameAvailabilities(IItemType sut)
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                Modification = new ItemTypeModification(ExpectedResult.Id, ExpectedResult.Name,
                    sut.Availabilities);
            }

            public void SetupValidationSuccess()
            {
                TestPropertyNotSetException.ThrowIfNull(Modification);

                ValidatorMock.SetupValidateAsync(Modification.Availabilities);
            }

            public void SetupValidationFailure()
            {
                TestPropertyNotSetException.ThrowIfNull(Modification);

                ExpectedException = new InvalidOperationException("injected");

                ValidatorMock
                    .SetupValidateAsyncAnd(Modification.Availabilities)
                    .ThrowsAsync(ExpectedException);
            }
        }
    }

    public class UpdateAsync
    {
        private readonly UpdateAsyncFixture _fixture = new();

        [Fact]
        public async Task UpdateAsync_WithValidationSuccess_ShouldReturnExpectedResult()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupUpdate();
            _fixture.SetupExpectedResult(sut);
            _fixture.SetupValidationSuccess();
            var expectedSut = sut.DeepClone();

            TestPropertyNotSetException.ThrowIfNull(_fixture.TypeUpdate);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await sut.UpdateAsync(_fixture.TypeUpdate, _fixture.ValidatorMock.Object);

            // Assert
            sut.Should().BeEquivalentTo(expectedSut);
            result.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt.Excluding(info => info.Path == "Id"));
            result.Id.Should().NotBe(sut.Id);
        }

        [Fact]
        public async Task UpdateAsync_WithValidationFailed_ShouldThrow()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupUpdate();
            _fixture.SetupValidationFailure();

            TestPropertyNotSetException.ThrowIfNull(_fixture.TypeUpdate);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedException);

            // Act
            var func = async () => await sut.UpdateAsync(_fixture.TypeUpdate, _fixture.ValidatorMock.Object);

            // Assert
            await func.Should().ThrowExactlyAsync<InvalidOperationException>()
                .WithMessage(_fixture.ExpectedException.Message);
        }

        [Fact]
        public async Task UpdateAsync_WithTypeDeleted_ShouldThrow()
        {
            // Arrange
            _fixture.SetupDeleted();
            var sut = _fixture.CreateSut();
            _fixture.SetupUpdate();

            TestPropertyNotSetException.ThrowIfNull(_fixture.TypeUpdate);

            // Act
            var func = async () => await sut.UpdateAsync(_fixture.TypeUpdate, _fixture.ValidatorMock.Object);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.CannotModifyDeletedItemType);
        }

        [Fact]
        public async Task UpdateAsync_WithEmptyAvailabilities_ShouldThrow()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupUpdateWithEmptyAvailabilities();

            TestPropertyNotSetException.ThrowIfNull(_fixture.TypeUpdate);

            // Act
            var func = async () => await sut.UpdateAsync(_fixture.TypeUpdate, _fixture.ValidatorMock.Object);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.CannotUpdateItemTypeWithoutAvailabilities);
        }

        private sealed class UpdateAsyncFixture : ItemTypeFixture
        {
            public ValidatorMock ValidatorMock { get; } = new(MockBehavior.Strict);
            public ItemTypeUpdate? TypeUpdate { get; private set; }
            public Exception? ExpectedException { get; private set; }
            public ItemType? ExpectedResult { get; private set; }

            public void SetupUpdate()
            {
                TypeUpdate = new DomainTestBuilder<ItemTypeUpdate>().Create();
            }

            public void SetupUpdateWithEmptyAvailabilities()
            {
                TypeUpdate = new DomainTestBuilder<ItemTypeUpdate>()
                    .FillConstructorWith("availabilities", Enumerable.Empty<ItemAvailability>()).Create();
            }

            public void SetupExpectedResult(IItemType sut)
            {
                TestPropertyNotSetException.ThrowIfNull(TypeUpdate);

                ExpectedResult = new ItemTypeBuilder()
                    .WithPredecessorId(sut.Id)
                    .WithIsDeleted(false)
                    .WithName(TypeUpdate.Name)
                    .WithAvailabilities(TypeUpdate.Availabilities)
                    .Create();
            }

            public void SetupValidationSuccess()
            {
                TestPropertyNotSetException.ThrowIfNull(TypeUpdate);

                ValidatorMock.SetupValidateAsync(TypeUpdate.Availabilities);
            }

            public void SetupValidationFailure()
            {
                TestPropertyNotSetException.ThrowIfNull(TypeUpdate);

                ExpectedException = new InvalidOperationException("injected");

                ValidatorMock
                    .SetupValidateAsyncAnd(TypeUpdate.Availabilities)
                    .ThrowsAsync(ExpectedException);
            }
        }
    }

    public class Update_Price
    {
        private readonly UpdateFixture _fixture = new();

        [Fact]
        public void Update_WithTypeDeleted_ShouldThrow()
        {
            // Arrange
            _fixture.SetupDeleted();
            var sut = _fixture.CreateSut();
            _fixture.SetupPrice();
            _fixture.SetupStoreId(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var func = () => sut.Update(_fixture.StoreId.Value, _fixture.Price.Value);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.CannotModifyDeletedItemType);
        }

        [Fact]
        public void Update_WithNotAvailableAtStore_ShouldThrow()
        {
            // Arrange
            _fixture.SetupPrice();
            _fixture.SetupInvalidStoreId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var func = () => sut.Update(_fixture.StoreId.Value, _fixture.Price.Value);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.ItemTypeAtStoreNotAvailable);
        }

        [Fact]
        public void Update_WithValidStoreId_ShouldUpdatePrice()
        {
            // Arrange
            _fixture.SetupPrice();
            var sut = _fixture.CreateSut();
            _fixture.SetupStoreId(sut);
            _fixture.SetupExpectedResult(sut);
            var expectedSut = sut.DeepClone();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.Update(_fixture.StoreId.Value, _fixture.Price.Value);

            // Assert
            sut.Should().BeEquivalentTo(expectedSut);
            result.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt.Excluding(info => info.Path == "Id"));
            result.Id.Should().NotBe(sut.Id);
        }

        private sealed class UpdateFixture : ItemTypeFixture
        {
            private readonly CommonFixture _fixture = new();
            private ItemAvailability? _chosenAvailability;

            public Price? Price { get; private set; }
            public StoreId? StoreId { get; private set; }
            public ItemType? ExpectedResult { get; private set; }

            public void SetupPrice()
            {
                Price = new DomainTestBuilder<Price>().Create();
            }

            public void SetupInvalidStoreId()
            {
                StoreId = Domain.Stores.Models.StoreId.New;
            }

            public void SetupStoreId(IItemType sut)
            {
                _chosenAvailability = _fixture.ChooseRandom(sut.Availabilities);
                StoreId = _chosenAvailability.StoreId;
            }

            public void SetupExpectedResult(IItemType sut)
            {
                TestPropertyNotSetException.ThrowIfNull(_chosenAvailability);
                TestPropertyNotSetException.ThrowIfNull(Price);

                var availabilities = sut.Availabilities.Except(_chosenAvailability.ToMonoList()).ToList();
                availabilities.Add(_chosenAvailability with { Price = Price.Value });

                ExpectedResult = new ItemTypeBuilder()
                    .WithName(sut.Name)
                    .WithPredecessorId(sut.Id)
                    .WithIsDeleted(false)
                    .WithAvailabilities(availabilities)
                    .Create();
            }
        }
    }

    public class Update
    {
        private readonly UpdateFixture _fixture = new();

        [Fact]
        public void Update_WithTypeDeleted_ShouldThrow()
        {
            // Arrange
            _fixture.SetupDeleted();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var func = () => sut.Update();

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.CannotModifyDeletedItemType);
        }

        [Fact]
        public void Update_WithValidStoreId_ShouldUpdatePrice()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);
            var expectedSut = sut.DeepClone();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.Update();

            // Assert
            sut.Should().BeEquivalentTo(expectedSut);
            result.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt.Excluding(info => info.Path == "Id"));
            result.Id.Should().NotBe(sut.Id);
        }

        private sealed class UpdateFixture : ItemTypeFixture
        {
            public ItemType? ExpectedResult { get; private set; }

            public void SetupExpectedResult(IItemType sut)
            {
                ExpectedResult = new ItemTypeBuilder()
                    .WithName(sut.Name)
                    .WithPredecessorId(sut.Id)
                    .WithIsDeleted(false)
                    .WithAvailabilities(sut.Availabilities)
                    .Create();
            }
        }
    }

    public class TransferToDefaultSection
    {
        private readonly TransferToDefaultSectionFixture _fixture = new();

        [Fact]
        public void TransferToDefaultSection_WithTypeDeleted_ShouldThrow()
        {
            // Arrange
            _fixture.SetupDeleted();
            var sut = _fixture.CreateSut();
            _fixture.SetupOldSectionId(sut);
            _fixture.SetupNewSectionId();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewSectionId);

            // Act
            var func = () => sut.TransferToDefaultSection(_fixture.OldSectionId.Value, _fixture.NewSectionId.Value);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.CannotModifyDeletedItemType);
        }

        [Fact]
        public void TransferToDefaultSection_WithInvalidOldSectionId_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupInvalidOldSectionId();
            _fixture.SetupNewSectionId();
            var sut = _fixture.CreateSut();
            var expectedSut = sut.DeepClone();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewSectionId);

            // Act
            var result = sut.TransferToDefaultSection(_fixture.OldSectionId.Value, _fixture.NewSectionId.Value);

            // Assert
            sut.Should().BeEquivalentTo(expectedSut);
            result.Should().Be(sut);
        }

        [Fact]
        public void TransferToDefaultSection_WithValidOldSectionId_ShouldUpdateSection()
        {
            // Arrange
            _fixture.SetupNewSectionId();
            var sut = _fixture.CreateSut();
            _fixture.SetupOldSectionId(sut);
            _fixture.SetupExpectedResult(sut);

            var expectedSut = sut.DeepClone();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.TransferToDefaultSection(_fixture.OldSectionId.Value, _fixture.NewSectionId.Value);

            // Assert
            sut.Should().BeEquivalentTo(expectedSut);
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class TransferToDefaultSectionFixture : ItemTypeFixture
        {
            private readonly CommonFixture _commonFixture = new();
            private ItemAvailability? _choseAvailability;

            public SectionId? OldSectionId { get; private set; }
            public SectionId? NewSectionId { get; private set; }
            public ItemType? ExpectedResult { get; private set; }

            public void SetupInvalidOldSectionId()
            {
                OldSectionId = SectionId.New;
            }

            public void SetupOldSectionId(IItemType sut)
            {
                _choseAvailability = _commonFixture.ChooseRandom(sut.Availabilities);
                OldSectionId = _choseAvailability.DefaultSectionId;
            }

            public void SetupNewSectionId()
            {
                NewSectionId = SectionId.New;
            }

            public void SetupExpectedResult(IItemType sut)
            {
                TestPropertyNotSetException.ThrowIfNull(_choseAvailability);
                TestPropertyNotSetException.ThrowIfNull(NewSectionId);

                var availabilities = sut.Availabilities.Except(_choseAvailability.ToMonoList()).ToList();
                availabilities.Add(_choseAvailability with { DefaultSectionId = NewSectionId.Value });

                ExpectedResult = new ItemTypeBuilder()
                    .WithId(sut.Id)
                    .WithName(sut.Name)
                    .WithPredecessorId(sut.PredecessorId)
                    .WithIsDeleted(false)
                    .WithAvailabilities(availabilities)
                    .Create();
            }
        }
    }

    public class Delete
    {
        private readonly DeleteFixture _fixture = new();

        [Fact]
        public void Delete_WithTypeDeleted_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupDeleted();
            var sut = _fixture.CreateSut();
            var expectedSut = sut.DeepClone();

            // Act
            var result = sut.Delete(out var resultEvent);

            // Assert
            result.Should().BeEquivalentTo(expectedSut);
            resultEvent.Should().BeNull();
        }

        [Fact]
        public void Delete_WithTypeNotDeleted_ShouldReturnExpectedResult()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);
            _fixture.SetupExpectedEvent(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedEvent);

            // Act
            var result = sut.Delete(out var resultEvent);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
            resultEvent.Should().BeEquivalentTo(_fixture.ExpectedEvent);
        }

        private sealed class DeleteFixture : ItemTypeFixture
        {
            public ItemType? ExpectedResult { get; private set; }
            public ItemTypeDeletedDomainEvent? ExpectedEvent { get; private set; }

            public void SetupExpectedResult(IItemType sut)
            {
                ExpectedResult = new ItemTypeBuilder()
                    .WithId(sut.Id)
                    .WithName(sut.Name)
                    .WithPredecessorId(sut.PredecessorId)
                    .WithIsDeleted(true)
                    .WithAvailabilities(sut.Availabilities)
                    .Create();
            }

            public void SetupExpectedEvent(IItemType sut)
            {
                ExpectedEvent = new ItemTypeDeletedDomainEvent(sut.Id);
            }
        }
    }

    public class RemoveAvailabilitiesFor
    {
        private readonly RemoveAvailabilitiesForFixture _fixture = new();

        [Fact]
        public void RemoveAvailabilitiesFor_WithInvalidStoreId_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupInvalidStoreId();
            var sut = _fixture.CreateSut();
            var expectedSut = sut.DeepClone();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);

            // Act
            var result = sut.RemoveAvailabilitiesFor(_fixture.StoreId.Value, out var resultEvents);

            // Assert
            result.Should().BeEquivalentTo(expectedSut);
            resultEvents.Should().BeEmpty();
        }

        [Fact]
        public void RemoveAvailabilitiesFor_WithOneAvailability_ShouldDeleteType()
        {
            // Arrange
            _fixture.SetupOneAvailability();
            var sut = _fixture.CreateSut();
            _fixture.SetupStoreId(sut);
            _fixture.SetupExpectedResultWithOneAvailability(sut);
            _fixture.SetupExpectedEventForOneAvailability(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedEvent);

            // Act
            var result = sut.RemoveAvailabilitiesFor(_fixture.StoreId.Value, out var resultEvents);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);

            var resultEventsList = resultEvents.ToList();
            resultEventsList.Should().HaveCount(1);
            resultEventsList.First().Should().BeOfType<ItemTypeDeletedDomainEvent>();

            var resultEvent = (ItemTypeDeletedDomainEvent)resultEventsList.First();
            resultEvent.Should().BeEquivalentTo((ItemTypeDeletedDomainEvent)_fixture.ExpectedEvent);
        }

        [Fact]
        public void RemoveAvailabilitiesFor_WithMultipleAvailabilities_ShouldRemoveAvailability()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupStoreId(sut);
            _fixture.SetupExpectedResultWithMultipleAvailabilities(sut);
            _fixture.SetupExpectedEventForMultipleAvailabilities(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedEvent);

            // Act
            var result = sut.RemoveAvailabilitiesFor(_fixture.StoreId.Value, out var resultEvents);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);

            var resultEventsList = resultEvents.ToList();
            resultEventsList.Should().HaveCount(1);
            resultEventsList.First().Should().BeOfType<ItemTypeAvailabilityDeletedDomainEvent>();

            var resultEvent = (ItemTypeAvailabilityDeletedDomainEvent)resultEventsList.First();
            resultEvent.Should().BeEquivalentTo((ItemTypeAvailabilityDeletedDomainEvent)_fixture.ExpectedEvent);
        }

        private sealed class RemoveAvailabilitiesForFixture : ItemTypeFixture
        {
            private readonly CommonFixture _commonFixture = new();
            private ItemAvailability? _chosenAvailability;

            public StoreId? StoreId { get; private set; }
            public ItemType? ExpectedResult { get; private set; }
            public IDomainEvent? ExpectedEvent { get; private set; }

            public void SetupInvalidStoreId()
            {
                StoreId = Domain.Stores.Models.StoreId.New;
            }

            public void SetupStoreId(IItemType sut)
            {
                _chosenAvailability = _commonFixture.ChooseRandom(sut.Availabilities);
                StoreId = _chosenAvailability.StoreId;
            }

            public void SetupOneAvailability()
            {
                var availability = new ItemAvailabilityBuilder().Create();
                Builder.WithAvailabilities(availability.ToMonoList());
            }

            public void SetupExpectedResultWithMultipleAvailabilities(IItemType sut)
            {
                TestPropertyNotSetException.ThrowIfNull(_chosenAvailability);

                var availabilities = sut.Availabilities.Except(_chosenAvailability.ToMonoList()).ToList();

                ExpectedResult = new ItemTypeBuilder()
                    .WithId(sut.Id)
                    .WithName(sut.Name)
                    .WithPredecessorId(sut.PredecessorId)
                    .WithIsDeleted(false)
                    .WithAvailabilities(availabilities)
                    .Create();
            }

            public void SetupExpectedResultWithOneAvailability(IItemType sut)
            {
                ExpectedResult = new ItemTypeBuilder()
                    .WithId(sut.Id)
                    .WithName(sut.Name)
                    .WithPredecessorId(sut.PredecessorId)
                    .WithIsDeleted(true)
                    .WithAvailabilities(sut.Availabilities)
                    .Create();
            }

            public void SetupExpectedEventForMultipleAvailabilities(IItemType sut)
            {
                TestPropertyNotSetException.ThrowIfNull(_chosenAvailability);
                ExpectedEvent = new ItemTypeAvailabilityDeletedDomainEvent(sut.Id, _chosenAvailability);
            }

            public void SetupExpectedEventForOneAvailability(IItemType sut)
            {
                ExpectedEvent = new ItemTypeDeletedDomainEvent(sut.Id);
            }
        }
    }

    public abstract class ItemTypeFixture
    {
        protected readonly ItemTypeBuilder Builder = new();

        protected ItemTypeFixture()
        {
            Builder.WithIsDeleted(false);
        }

        public void SetupDeleted()
        {
            Builder.WithIsDeleted(true);
        }

        public ItemType CreateSut()
        {
            return Builder
                .Create();
        }
    }
}