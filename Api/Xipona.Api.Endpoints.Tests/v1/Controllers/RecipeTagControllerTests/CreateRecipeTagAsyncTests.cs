using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.RecipeTags.Commands.CreateRecipeTag;
using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Commands;
using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Queries.GetAll;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.RecipeTagControllerTests;

public class CreateRecipeTagAsyncTests : ControllerCommandWithReturnTypeTestsBase<RecipeTagController,
    CreateRecipeTagCommand, IRecipeTag, RecipeTagContract,
    CreateRecipeTagAsyncTests.CreateRecipeTagAsyncFixture>
{
    public CreateRecipeTagAsyncTests() : base(new CreateRecipeTagAsyncFixture())
    {
    }

    public sealed class CreateRecipeTagAsyncFixture : ControllerCommandWithReturnTypeFixtureBase
    {
        private CreateRecipeTagContract? _contract;

        public CreateRecipeTagAsyncFixture()
        {
            PossibleResultsList.Add(new UnprocessableEntityStatusResult());
            PossibleResultsList.Add(new CreatedStatusResult());
        }

        public override MethodInfo Method =>
            typeof(RecipeTagController).GetMethod(nameof(RecipeTagController.CreateRecipeTagAsync))!;

        public override RecipeTagController CreateSut()
        {
            return new RecipeTagController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(RecipeTagController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            return await sut.CreateRecipeTagAsync(_contract);
        }

        public override void SetupParameters()
        {
            _contract = new DomainTestBuilder<CreateRecipeTagContract>().Create();
        }

        public override void SetupCommand()
        {
            Command = new DomainTestBuilder<CreateRecipeTagCommand>().Create();
        }

        public override void SetupCommandConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            TestPropertyNotSetException.ThrowIfNull(Command);

            EndpointConvertersMock.SetupToDomain(_contract, Command);
        }
    }
}