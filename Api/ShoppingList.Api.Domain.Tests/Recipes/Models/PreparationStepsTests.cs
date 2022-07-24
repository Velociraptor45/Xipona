using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.Models;

public class PreparationStepsTests
{
    private readonly PreparationStepsFixture _fixture;

    public PreparationStepsTests()
    {
        _fixture = new PreparationStepsFixture();
    }

    [Fact]
    public void Ctor_WithValidSortingIndexes_ShouldNotThrow()
    {
        // Arrange
        _fixture.SetupValidPreparationSteps();

        TestPropertyNotSetException.ThrowIfNull(_fixture.PreparationSteps);

        // Act
        var func = () => new PreparationSteps(_fixture.PreparationSteps);

        // Assert
        func.Should().NotThrow();
    }

    [Fact]
    public void Ctor_WithDuplicatedSortingIndexes_ShouldThrow()
    {
        // Arrange
        _fixture.SetupDuplicatedPreparationSteps();

        TestPropertyNotSetException.ThrowIfNull(_fixture.PreparationSteps);

        // Act
        var func = () => new PreparationSteps(_fixture.PreparationSteps);

        // Assert
        func.Should().ThrowDomainException(ErrorReasonCode.DuplicatedSortingIndex);
    }

    [Fact]
    public void AsReadOnly_WithValidData_ShouldReturnExpectedResult()
    {
        // Arrange
        _fixture.SetupValidPreparationSteps();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.PreparationSteps);

        // Act
        var result = sut.AsReadOnly();

        // Assert
        result.Should().BeEquivalentTo(_fixture.PreparationSteps);
    }

    private sealed class PreparationStepsFixture
    {
        public IReadOnlyCollection<IPreparationStep>? PreparationSteps { get; private set; }

        public void SetupValidPreparationSteps()
        {
            var list = new List<IPreparationStep>();

            for (int i = 0; i < 3; i++)
            {
                list.Add(new PreparationStepBuilder().WithSortingIndex(i).Create());
            }

            PreparationSteps = list;
        }

        public void SetupDuplicatedPreparationSteps()
        {
            var list = new List<IPreparationStep>();

            for (int i = 0; i < 3; i++)
            {
                list.Add(new PreparationStepBuilder().WithSortingIndex(i).Create());
            }
            list.Add(new PreparationStepBuilder().WithSortingIndex(1).Create());

            PreparationSteps = list;
        }

        public PreparationSteps CreateSut()
        {
            TestPropertyNotSetException.ThrowIfNull(PreparationSteps);
            return new PreparationSteps(PreparationSteps);
        }
    }
}