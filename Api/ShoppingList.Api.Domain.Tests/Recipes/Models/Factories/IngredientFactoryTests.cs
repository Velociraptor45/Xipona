using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.Models.Factories;

public class IngredientFactoryTests
{
    public class Create
    {
        private readonly CreateFixture _fixture;

        public Create()
        {
            _fixture = new CreateFixture();
        }

        [Fact]
        public void CreateNew_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.Create(
                _fixture.ExpectedResult.Id,
                _fixture.ExpectedResult.ItemCategoryId,
                _fixture.ExpectedResult.QuantityType,
                _fixture.ExpectedResult.Quantity);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class CreateFixture : IngredientFactoryFixture
        {
            public IIngredient? ExpectedResult { get; private set; }

            public void SetupExpectedResult()
            {
                ExpectedResult = new IngredientBuilder().Create();
            }
        }
    }

    public class CreateNew
    {
        private readonly CreateNewFixture _fixture;

        public CreateNew()
        {
            _fixture = new CreateNewFixture();
        }

        [Fact]
        public async Task CreateNew_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupCreation();
            _fixture.SetupValidationSuccessful();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Creation);

            // Act
            var result = await sut.CreateNewAsync(_fixture.Creation);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt.Excluding(i => i.Id));
        }

        [Fact]
        public async Task CreateNew_WithInvalidItemCategoryId_ShouldThrow()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupCreation();
            _fixture.SetupValidationFailed();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Creation);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedException);

            // Act
            var func = async () => await sut.CreateNewAsync(_fixture.Creation);

            // Assert
            (await func.Should().ThrowExactlyAsync<DomainException>()).Where(e => e == _fixture.ExpectedException);
        }

        private sealed class CreateNewFixture : IngredientFactoryFixture
        {
            public IIngredient? ExpectedResult { get; private set; }
            public DomainException? ExpectedException { get; private set; }
            public IngredientCreation? Creation { get; private set; }

            public void SetupExpectedResult()
            {
                ExpectedResult = new IngredientBuilder().Create();
            }

            public void SetupCreation()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                Creation = new IngredientCreation(ExpectedResult.ItemCategoryId,
                    ExpectedResult.QuantityType,
                    ExpectedResult.Quantity);
            }

            public void SetupValidationSuccessful()
            {
                TestPropertyNotSetException.ThrowIfNull(Creation);
                ValidatorMock.SetupValidateAsync(Creation.ItemCategoryId);
            }

            public void SetupValidationFailed()
            {
                TestPropertyNotSetException.ThrowIfNull(Creation);
                ExpectedException = new DomainException(new ItemCategoryNotFoundReason(Creation.ItemCategoryId));
                ValidatorMock.SetupValidateAsyncAnd(Creation.ItemCategoryId).ThrowsAsync(ExpectedException);
            }
        }
    }

    private abstract class IngredientFactoryFixture
    {
        protected readonly ValidatorMock ValidatorMock = new(MockBehavior.Strict);

        public IngredientFactory CreateSut()
        {
            return new IngredientFactory(_ => ValidatorMock.Object, default);
        }
    }
}