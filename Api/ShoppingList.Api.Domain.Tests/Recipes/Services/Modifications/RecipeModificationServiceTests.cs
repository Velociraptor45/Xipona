using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Ports;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.Services.Modifications;

public class RecipeModificationServiceTests
{
    private readonly RecipeModificationServiceFixture _fixture;

    public RecipeModificationServiceTests()
    {
        _fixture = new RecipeModificationServiceFixture();
    }

    [Fact]
    public async Task ModifyAsync_WithValidRecipeId_ShouldModifyRecipe()
    {
        // Arrange
        _fixture.SetupModification();
        _fixture.SetupRecipe();
        _fixture.SetupFindingRecipe();
        _fixture.SetupModifyingRecipe();
        _fixture.SetupStoringRecipe();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyModifyingRecipe();
    }

    [Fact]
    public async Task ModifyAsync_WithValidRecipeId_ShouldStoreRecipe()
    {
        // Arrange
        _fixture.SetupModification();
        _fixture.SetupRecipe();
        _fixture.SetupFindingRecipe();
        _fixture.SetupModifyingRecipe();
        _fixture.SetupStoringRecipe();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyStoringRecipe();
    }

    [Fact]
    public async Task ModifyAsync_WithInvalidRecipeId_ShouldThrowDomainException()
    {
        // Arrange
        _fixture.SetupModification();
        _fixture.SetupNotFindingRecipe();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        var func = async () => await sut.ModifyAsync(_fixture.Modification);

        // Assert
        await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.RecipeNotFound);
    }

    private sealed class RecipeModificationServiceFixture
    {
        private readonly RecipeRepositoryMock _recipeRepositoryMock = new(MockBehavior.Strict);
        private readonly ValidatorMock _validatorMock = new(MockBehavior.Strict);
        private RecipeMock? _recipeMock;

        public RecipeModificationService CreateSut()
        {
            return new RecipeModificationService(
                _ => _recipeRepositoryMock.Object,
                _ => _validatorMock.Object,
                default);
        }

        public RecipeModification? Modification { get; private set; }

        public void SetupModification()
        {
            Modification = new DomainTestBuilder<RecipeModification>().Create();
        }

        public void SetupRecipe()
        {
            _recipeMock = new RecipeMock(MockBehavior.Strict);
        }

        public void SetupModifyingRecipe()
        {
            TestPropertyNotSetException.ThrowIfNull(_recipeMock);
            TestPropertyNotSetException.ThrowIfNull(Modification);

            _recipeMock.SetupModifyAsync(Modification, _validatorMock.Object);
        }

        public void SetupFindingRecipe()
        {
            TestPropertyNotSetException.ThrowIfNull(Modification);
            TestPropertyNotSetException.ThrowIfNull(_recipeMock);

            _recipeRepositoryMock.SetupFindByAsync(Modification.Id, _recipeMock.Object);
        }

        public void SetupNotFindingRecipe()
        {
            TestPropertyNotSetException.ThrowIfNull(Modification);

            _recipeRepositoryMock.SetupFindByAsync(Modification.Id, null);
        }

        public void SetupStoringRecipe()
        {
            TestPropertyNotSetException.ThrowIfNull(_recipeMock);

            _recipeRepositoryMock.SetupStoreAsync(_recipeMock.Object, _recipeMock.Object);
        }

        public void VerifyModifyingRecipe()
        {
            TestPropertyNotSetException.ThrowIfNull(_recipeMock);
            TestPropertyNotSetException.ThrowIfNull(Modification);

            _recipeMock.VerifyModifyAsync(Modification, _validatorMock.Object, Times.Once);
        }

        public void VerifyStoringRecipe()
        {
            TestPropertyNotSetException.ThrowIfNull(_recipeMock);
            _recipeRepositoryMock.VerifyStoreAsync(_recipeMock.Object, Times.Once);
        }
    }
}