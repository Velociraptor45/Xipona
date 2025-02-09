using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProjectHermes.Xipona.Api.ApplicationServices.RecipeTags.Commands.CreateRecipeTag;
using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Commands;
using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Queries.GetAll;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Net.Http;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Endpoints.RecipeTagTests;

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
                default);
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