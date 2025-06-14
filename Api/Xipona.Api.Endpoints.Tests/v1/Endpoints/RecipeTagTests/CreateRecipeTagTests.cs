using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Xipona.Api.ApplicationServices.RecipeTags.Commands.CreateRecipeTag;
using Xipona.Api.Contracts.RecipeTags.Commands;
using Xipona.Api.Contracts.RecipeTags.Queries.GetAll;
using Xipona.Api.Domain.RecipeTags.Models;
using Xipona.Api.Domain.TestKit.Common;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Endpoints.Tests.Common;
using Xipona.Api.Endpoints.Tests.Common.StatusResults;
using Xipona.Api.TestTools.Exceptions;
using System.Net.Http;

namespace Xipona.Api.Endpoints.Tests.v1.Endpoints.RecipeTagTests;

public class CreateRecipeTagTests : EndpointCommandWithReturnTypeTestsBase<CreateRecipeTagContract,
    CreateRecipeTagCommand, IRecipeTag, RecipeTagContract,
    CreateRecipeTagTests.CreateRecipeTagFixture>
{
    public CreateRecipeTagTests() : base(new CreateRecipeTagFixture())
    {
    }

    public sealed class CreateRecipeTagFixture : EndpointCommandWithReturnTypeFixtureBase
    {
        private CreateRecipeTagContract? _contract;

        public CreateRecipeTagFixture()
        {
            PossibleResultsList.Add(new UnprocessableEntityStatusResult());
            PossibleResultsList.Add(new CreatedStatusResult());
        }

        public override string RoutePattern => "/v1/recipe-tags";
        public override HttpMethod HttpMethod => HttpMethod.Post;

        public override async Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            return await RecipeTagEndpoints.CreateRecipeTag(
                _contract,
                CommandDispatcherMock.Object,
                ErrorConverterMock.Object,
                CommandConverterMock.Object,
                ContractConverterMock.Object,
                TestContext.Current.CancellationToken);
        }

        public override void SetupParameters()
        {
            _contract = new DomainTestBuilder<CreateRecipeTagContract>().Create();
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterRecipeTagEndpoints();
        }

        public override CreateRecipeTagContract GetCommandConverterInput()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            return _contract;
        }
    }
}