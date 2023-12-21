using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Commands.ModifyRecipe;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.ModifyRecipe;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.RecipeControllerTests;

public class ModifyRecipeAsyncTests : ControllerCommandTestsBase<RecipeController, ModifyRecipeCommand, bool,
        ModifyRecipeAsyncTests.ModifyRecipeAsyncFixture>
{
    public ModifyRecipeAsyncTests() : base(new ModifyRecipeAsyncFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.RecipeNotFound)]
    [InlineData(ErrorReasonCode.IngredientNotFound)]
    [InlineData(ErrorReasonCode.PreparationStepNotFound)]
    public async Task EndpointCall_WithDomainException_ShouldReturnNotFound(ErrorReasonCode errorCode)
    {
        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupCommand();
        Fixture.SetupCommandConverter();
        Fixture.SetupDomainException(errorCode);
        Fixture.SetupDomainExceptionInCommandDispatcher();
        Fixture.SetupExpectedErrorContract();
        Fixture.SetupErrorConversion();
        var sut = Fixture.CreateSut();

        // Act
        var result = await Fixture.ExecuteTestMethod(sut);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var unprocessableEntity = result as NotFoundObjectResult;
        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class ModifyRecipeAsyncFixture : ControllerCommandFixtureBase
    {
        private ModifyRecipeContract? _contract;
        private readonly Guid _recipeId = Guid.NewGuid();

        public ModifyRecipeAsyncFixture()
        {
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(
                ErrorReasonCode.RecipeNotFound,
                ErrorReasonCode.IngredientNotFound,
                ErrorReasonCode.PreparationStepNotFound));
            PossibleResultsList.Add(new NoContentStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
        }

        public override MethodInfo Method =>
            typeof(RecipeController).GetMethod(nameof(RecipeController.ModifyRecipeAsync))!;

        public override RecipeController CreateSut()
        {
            return new RecipeController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(RecipeController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            return await sut.ModifyRecipeAsync(_recipeId, _contract);
        }

        public override void SetupParameters()
        {
            _contract = new TestBuilder<ModifyRecipeContract>().Create();
        }

        public override void SetupCommand()
        {
            Command = new DomainTestBuilder<ModifyRecipeCommand>().Create();
        }

        public override void SetupCommandConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            TestPropertyNotSetException.ThrowIfNull(Command);

            EndpointConvertersMock.SetupToDomain((_recipeId, _contract), Command);
        }
    }
}