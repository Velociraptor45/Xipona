using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProjectHermes.Xipona.Api.ApplicationServices.RecipeTags.Queries.GetAll;
using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Queries.GetAll;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Endpoints.RecipeTagTests;

public class GetAllRecipeTagsTests : EndpointEnumerableQueryNoConverterTestsBase<
    GetAllQuery, IRecipeTag, RecipeTagContract, GetAllRecipeTagsTests.GetAllRecipeTagsFixture>
{
    public GetAllRecipeTagsTests() : base(new GetAllRecipeTagsFixture())
    {
    }

    public sealed class GetAllRecipeTagsFixture : EndpointEnumerableQueryNoConverterFixtureBase
    {
        public GetAllRecipeTagsFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override string RoutePattern => "/v1/recipe-tags/all";

        public override async Task<IResult> ExecuteTestMethod()
        {
            return await RecipeTagEndpoints.GetAllRecipeTags(
                QueryDispatcherMock.Object,
                ContractConverterMock.Object,
                default);
        }

        public override void SetupParameters()
        {
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterRecipeTagEndpoints();
        }

        public override void SetupQuery()
        {
            Query = new GetAllQuery();
        }
    }
}