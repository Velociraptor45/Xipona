using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.Models;

public class IngredientTests
{
    public class Modify
    {
        private readonly ModifyFixture _fixture;

        public Modify()
        {
            _fixture = new ModifyFixture();
        }

        [Fact]
        public async Task Modify_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupExpectedResult();
            _fixture.SetupModification();
            _fixture.SetupValidationSuccess();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

            // Act
            var result = await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public async Task Modify_WithInvalidItemCategory_ShouldThrow()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupExpectedResult();
            _fixture.SetupModification();
            _fixture.SetupValidationFailure();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedException);

            // Act
            var func = async () => await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            await func.Should().ThrowExactlyAsync<InvalidOperationException>()
                .WithMessage(_fixture.ExpectedException.Message);
        }

        private sealed class ModifyFixture : IngredientFixture
        {
            public ValidatorMock ValidatorMock { get; } = new(MockBehavior.Strict);
            public Ingredient? ExpectedResult { get; private set; }
            public IngredientModification? Modification { get; private set; }
            public InvalidOperationException? ExpectedException { get; private set; }

            public void SetupExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(Id);

                ExpectedResult = new IngredientBuilder()
                    .WithId(Id.Value)
                    .Create();
            }

            public void SetupModification()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                Modification = new IngredientModification(
                    ExpectedResult.Id,
                    ExpectedResult.ItemCategoryId,
                    ExpectedResult.QuantityType,
                    ExpectedResult.Quantity);
            }

            public void SetupValidationSuccess()
            {
                TestPropertyNotSetException.ThrowIfNull(Modification);

                ValidatorMock.SetupValidateAsync(Modification.ItemCategoryId);
            }

            public void SetupValidationFailure()
            {
                TestPropertyNotSetException.ThrowIfNull(Modification);

                ExpectedException = new InvalidOperationException("injected");

                ValidatorMock.SetupValidateAsyncAnd(Modification.ItemCategoryId)
                    .Throws(ExpectedException);
            }
        }
    }

    public abstract class IngredientFixture
    {
        protected IngredientId? Id;

        public void SetupId()
        {
            Id = IngredientId.New;
        }

        public Ingredient CreateSut()
        {
            var builder = new IngredientBuilder();

            if (Id is not null)
                builder.WithId(Id.Value);

            return builder.Create();
        }
    }
}