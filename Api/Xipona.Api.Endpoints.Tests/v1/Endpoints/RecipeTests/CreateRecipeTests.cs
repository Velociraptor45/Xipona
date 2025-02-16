using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Commands.CreateRecipe;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Commands.CreateRecipe;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Net.Http;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Endpoints.RecipeTests;

public class CreateRecipeTests :
    EndpointCommandWithReturnTypeTestsBase<CreateRecipeContract, CreateRecipeCommand, RecipeReadModel, RecipeContract,
        CreateRecipeTests.CreateRecipeFixture>
{
    public CreateRecipeTests() : base(new CreateRecipeFixture())
    {
    }

    public sealed class CreateRecipeFixture : EndpointCommandWithReturnTypeFixtureBase
    {
        private CreateRecipeContract? _contract;

        public CreateRecipeFixture()
        {
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(new List<ErrorReasonCode>
            {
                ErrorReasonCode.ItemNotFound
            }));
            PossibleResultsList.Add(new CreatedStatusResult());
        }

        public override string RoutePattern => "/v1/recipes";
        public override HttpMethod HttpMethod => HttpMethod.Post;

        public override async Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            return await RecipeEndpoints.CreateRecipe(
                _contract,
                CommandDispatcherMock.Object,
                CommandConverterMock.Object,
                ContractConverterMock.Object,
                ErrorConverterMock.Object,
                default);
        }

        public override void SetupParameters()
        {
            _contract = new TestBuilder<CreateRecipeContract>().Create();
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterRecipeEndpoints();
        }

        public override CreateRecipeContract GetCommandConverterInput()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            return _contract;
        }
    }
}