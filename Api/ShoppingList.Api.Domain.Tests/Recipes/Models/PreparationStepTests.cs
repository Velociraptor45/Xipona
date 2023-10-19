using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.Models;

public class PreparationStepTests
{
    public class Modify
    {
        private readonly ModifyFixture _fixture = new();

        [Fact]
        public void Modify_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupExpectedResult();
            _fixture.SetupModification();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

            // Act
            var result = sut.Modify(_fixture.Modification);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class ModifyFixture : PreparationStepFixture
        {
            public PreparationStep? ExpectedResult { get; private set; }
            public PreparationStepModification? Modification { get; private set; }

            public void SetupExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(Id);

                ExpectedResult = new PreparationStepBuilder()
                    .WithId(Id.Value)
                    .Create();
            }

            public void SetupModification()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                Modification = new PreparationStepModification(
                    ExpectedResult.Id,
                    ExpectedResult.Instruction,
                    ExpectedResult.SortingIndex);
            }
        }
    }

    public abstract class PreparationStepFixture
    {
        protected PreparationStepId? Id;

        public void SetupId()
        {
            Id = PreparationStepId.New;
        }

        public PreparationStep CreateSut()
        {
            var builder = new PreparationStepBuilder();

            if (Id is not null)
                builder.WithId(Id.Value);

            return builder.Create();
        }
    }
}