using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Models.Factories;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.Recipes.Models;

public class PreparationStepsTests
{
    private readonly PreparationStepsFixture _ctorFixture;

    public PreparationStepsTests()
    {
        _ctorFixture = new PreparationStepsFixture();
    }

    [Fact]
    public void Ctor_WithValidSortingIndexes_ShouldNotThrow()
    {
        // Arrange
        _ctorFixture.SetupValidPreparationSteps();

        // Act
        var func = () => new PreparationSteps(_ctorFixture.PreparationSteps, _ctorFixture.PreparationStepFactoryMock.Object);

        // Assert
        func.Should().NotThrow();
    }

    [Fact]
    public void Ctor_WithDuplicatedSortingIndexes_ShouldThrow()
    {
        // Arrange
        _ctorFixture.SetupDuplicatedPreparationSteps();

        // Act
        var func = () => new PreparationSteps(_ctorFixture.PreparationSteps, _ctorFixture.PreparationStepFactoryMock.Object);

        // Assert
        func.Should().ThrowDomainException(ErrorReasonCode.DuplicatedSortingIndex);
    }

    [Fact]
    public void AsReadOnly_WithValidData_ShouldReturnExpectedResult()
    {
        // Arrange
        _ctorFixture.SetupValidPreparationSteps();
        var sut = _ctorFixture.CreateSut();

        // Act
        var result = sut.AsReadOnly();

        // Assert
        result.Should().BeEquivalentTo(_ctorFixture.PreparationSteps);
    }

    public class ModifyMany
    {
        private readonly ModifyManyFixture _fixture;

        public ModifyMany()
        {
            _fixture = new ModifyManyFixture();
        }

        [Fact]
        public void ModifyMany_WithInvalidPreparationStepId_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupPreparationStepToModifyWithInvalidId();
            var sut = _fixture.CreateSut();

            // Act
            var func = () => sut.ModifyMany(_fixture.PreparationStepModifications);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.PreparationStepNotFound);
        }

        [Fact]
        public void ModifyMany_WithPreparationStepToModify_ShouldModifyPreparationStep()
        {
            // Arrange
            _fixture.SetupPreparationStepToModify();
            var sut = _fixture.CreateSut();

            // Act
            sut.ModifyMany(_fixture.PreparationStepModifications);

            // Assert
            _fixture.VerifyModifyingPreparationStepToModify();
        }

        [Fact]
        public void ModifyMany_WithPreparationStepToModify_ShouldModifyItselfCorrectly()
        {
            // Arrange
            _fixture.SetupPreparationStepToModify();
            var sut = _fixture.CreateSut();

            // Act
            sut.ModifyMany(_fixture.PreparationStepModifications);

            // Assert
            sut.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void ModifyMany_WithPreparationStepToCreate_ShouldModifyItselfCorrectly()
        {
            // Arrange
            _fixture.SetupPreparationStepToCreate();
            var sut = _fixture.CreateSut();

            // Act
            sut.ModifyMany(_fixture.PreparationStepModifications);

            // Assert
            sut.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void ModifyMany_WithPreparationStepToDelete_ShouldModifyItselfCorrectly()
        {
            // Arrange
            _fixture.SetupPreparationStepToDelete();
            var sut = _fixture.CreateSut();

            // Act
            sut.ModifyMany(_fixture.PreparationStepModifications);

            // Assert
            sut.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class ModifyManyFixture : PreparationStepsFixture
        {
            private readonly List<IPreparationStep> _expectedResult = new();
            private readonly List<PreparationStepModification> _modifications = new();

            private PreparationStepMock? _preparationStepMockToModify;
            private PreparationStepModification? _modificationForPreparationStepMockToModify;
            public IReadOnlyCollection<PreparationStepModification> PreparationStepModifications => _modifications;
            public IReadOnlyCollection<IPreparationStep> ExpectedResult => _expectedResult;

            public void SetupPreparationStepToModifyWithInvalidId()
            {
                _preparationStepMockToModify = new(new PreparationStepBuilder().Create(), MockBehavior.Strict);
                PreparationStepsList.Add(_preparationStepMockToModify.Object);

                _modificationForPreparationStepMockToModify = new DomainTestBuilder<PreparationStepModification>()
                    .Create();
                _modifications.Add(_modificationForPreparationStepMockToModify);
            }

            public void SetupPreparationStepToModify()
            {
                _preparationStepMockToModify = new(new PreparationStepBuilder().Create(), MockBehavior.Strict);
                var preparationStepId = _preparationStepMockToModify.Object.Id;
                PreparationStepsList.Add(_preparationStepMockToModify.Object);

                _modificationForPreparationStepMockToModify = new DomainTestBuilder<PreparationStepModification>()
                    .FillConstructorWith("id", (PreparationStepId?)preparationStepId)
                    .Create();
                _modifications.Add(_modificationForPreparationStepMockToModify);

                var modifiedPreparationStep = new PreparationStepBuilder().WithId(preparationStepId).Create();
                _expectedResult.Add(modifiedPreparationStep);

                _preparationStepMockToModify.SetupModify(_modificationForPreparationStepMockToModify,
                    modifiedPreparationStep);
            }

            public void SetupPreparationStepToCreate()
            {
                var modification = new DomainTestBuilder<PreparationStepModification>()
                    .FillConstructorWith("id", (PreparationStepId?)null)
                    .Create();
                _modifications.Add(modification);

                var createdPreparationStep = new PreparationStepBuilder().Create();
                _expectedResult.Add(createdPreparationStep);

                PreparationStepFactoryMock.SetupCreateNew(modification.Instruction, modification.SortingIndex,
                    createdPreparationStep);
            }

            public void SetupPreparationStepToDelete()
            {
                PreparationStepsList.Add(new PreparationStepBuilder().Create());
            }

            public void VerifyModifyingPreparationStepToModify()
            {
                TestPropertyNotSetException.ThrowIfNull(_preparationStepMockToModify);
                TestPropertyNotSetException.ThrowIfNull(_modificationForPreparationStepMockToModify);

                _preparationStepMockToModify.VerifyModify(_modificationForPreparationStepMockToModify, Times.Once);
            }
        }
    }

    private class PreparationStepsFixture
    {
        protected readonly List<IPreparationStep> PreparationStepsList = new();
        public IReadOnlyCollection<IPreparationStep> PreparationSteps => PreparationStepsList;
        public PreparationStepFactoryMock PreparationStepFactoryMock { get; } = new(MockBehavior.Strict);

        public void SetupValidPreparationSteps()
        {
            var list = new List<IPreparationStep>();

            for (int i = 0; i < 3; i++)
            {
                list.Add(new PreparationStepBuilder().WithSortingIndex(i).Create());
            }

            PreparationStepsList.AddRange(list);
        }

        public void SetupDuplicatedPreparationSteps()
        {
            var list = new List<IPreparationStep>();

            for (int i = 0; i < 3; i++)
            {
                list.Add(new PreparationStepBuilder().WithSortingIndex(i).Create());
            }
            list.Add(new PreparationStepBuilder().WithSortingIndex(1).Create());

            PreparationStepsList.AddRange(list);
        }

        public PreparationSteps CreateSut()
        {
            return new PreparationSteps(PreparationSteps, PreparationStepFactoryMock.Object);
        }
    }
}