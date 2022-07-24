using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.Models.Factories;

public class PreparationStepFactoryTests
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
                _fixture.ExpectedResult.Instruction,
                _fixture.ExpectedResult.SortingIndex);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class CreateFixture : PreparationStepFactoryFixture
        {
            public IPreparationStep? ExpectedResult { get; private set; }

            public void SetupExpectedResult()
            {
                ExpectedResult = new PreparationStepBuilder().Create();
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
        public void CreateNew_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupCreation();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Creation);

            // Act
            var result = sut.CreateNew(_fixture.Creation);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt.Excluding(i => i.Id));
        }

        private sealed class CreateNewFixture : PreparationStepFactoryFixture
        {
            public IPreparationStep? ExpectedResult { get; private set; }
            public PreparationStepCreation? Creation { get; private set; }

            public void SetupExpectedResult()
            {
                ExpectedResult = new PreparationStepBuilder().Create();
            }

            public void SetupCreation()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                Creation = new PreparationStepCreation(
                    ExpectedResult.Instruction,
                    ExpectedResult.SortingIndex);
            }
        }
    }

    private abstract class PreparationStepFactoryFixture
    {
        public PreparationStepFactory CreateSut()
        {
            return new PreparationStepFactory();
        }
    }
}