using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Xipona.Api.ApplicationServices.Recipes.Commands.CreateRecipe;
using Xipona.Api.Contracts.Recipes.Commands.CreateRecipe;
using Xipona.Api.Contracts.Recipes.Queries.Get;
using Xipona.Api.Core.TestKit;
using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Recipes.Services.Queries;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Endpoints.Tests.Common;
using Xipona.Api.Endpoints.Tests.Common.StatusResults;
using Xipona.Api.TestTools.Exceptions;
using System.Net.Http;

namespace Xipona.Api.Endpoints.Tests.v1.Endpoints.RecipeTests;

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