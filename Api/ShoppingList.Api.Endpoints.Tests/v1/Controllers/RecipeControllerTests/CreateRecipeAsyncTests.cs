using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Commands.CreateRecipe;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.CreateRecipe;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.RecipeControllerTests;

public class CreateRecipeAsyncTests :
    ControllerCommandWithReturnTypeTestsBase<RecipeController, CreateRecipeCommand, RecipeReadModel, RecipeContract,
        CreateRecipeAsyncTests.CreateRecipeAsyncFixture>
{
    public CreateRecipeAsyncTests() : base(new CreateRecipeAsyncFixture())
    {
    }

    public sealed class CreateRecipeAsyncFixture : ControllerCommandWithReturnTypeFixtureBase
    {
        private CreateRecipeContract? _contract;

        public CreateRecipeAsyncFixture()
        {
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(new List<ErrorReasonCode>
            {
                ErrorReasonCode.ItemNotFound
            }));
            PossibleResultsList.Add(new CreatedStatusResult());
        }

        public override MethodInfo Method =>
            typeof(RecipeController).GetMethod(nameof(RecipeController.CreateRecipeAsync))!;

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
            return await sut.CreateRecipeAsync(_contract);
        }

        public override void SetupParameters()
        {
            _contract = new TestBuilder<CreateRecipeContract>().Create();
        }

        public override void SetupCommand()
        {
            Command = new DomainTestBuilder<CreateRecipeCommand>().Create();
        }

        public override void SetupCommandConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            TestPropertyNotSetException.ThrowIfNull(Command);

            EndpointConvertersMock.SetupToDomain(_contract, Command);
        }
    }
}