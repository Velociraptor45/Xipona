using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.Models;

public class ItemTypeTests
{
    public class Update
    {
        private readonly UpdateFixture _fixture;

        public Update()
        {
            _fixture = new UpdateFixture();
        }

        [Fact]
        public void Update_WithNotAvailableAtStore_ShouldThrow()
        {
            // Arrange
            _fixture.SetupPrice();
            _fixture.SetupStoreId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var func = () => sut.Update(_fixture.StoreId.Value, _fixture.Price.Value);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.ItemTypeAtStoreNotAvailable);
        }

        private sealed class UpdateFixture : ItemTypeFixture
        {
            public Price? Price { get; private set; }
            public StoreId? StoreId { get; private set; }

            public void SetupPrice()
            {
                Price = new DomainTestBuilder<Price>().Create();
            }

            public void SetupStoreId()
            {
                StoreId = Domain.Stores.Models.StoreId.New;
            }
        }
    }

    public class ModifyAsync
    {
        private readonly ModifyAsyncFixture _fixture;

        public ModifyAsync()
        {
            _fixture = new ModifyAsyncFixture();
        }

        [Fact]
        public async Task ModifyAsync_WithValidationSuccess_ShouldReturnExpectedResult()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);
            _fixture.SetupModification();
            _fixture.SetupValidationSuccess();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
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
                    .Create();
            }

            public void SetupModification()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                Modification = new ItemTypeModification(ExpectedResult.Id, ExpectedResult.Name,
                    ExpectedResult.Availabilities);
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

    public abstract class ItemTypeFixture
    {
        protected readonly ItemTypeBuilder Builder = new();

        public ItemType CreateSut()
        {
            return Builder.Create();
        }
    }
}