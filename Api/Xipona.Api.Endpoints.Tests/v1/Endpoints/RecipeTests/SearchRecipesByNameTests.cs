using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Endpoints.RecipeTests;

public class SearchRecipesByNameTests : EndpointEnumerableQueryNoConverterTestsBase<SearchRecipesByNameQuery,
    RecipeSearchResult, RecipeSearchResultContract, SearchRecipesByNameTests.SearchRecipesByNameFixture>
{
    public SearchRecipesByNameTests() : base(new SearchRecipesByNameFixture())
    {
    }

    public sealed class SearchRecipesByNameFixture : EndpointEnumerableQueryNoConverterFixtureBase
    {
        private string? _searchInput;

        public SearchRecipesByNameFixture()
        {
            PossibleResultsList.Add(new UnprocessableEntityStatusResult());
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
            PossibleResultsList.Add(new BadRequestStatusResult());
        }

        public override string RoutePattern => "/v1/recipes/search-by-name";

        public override async Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_searchInput);

            return await RecipeEndpoints.SearchRecipesByName(
                _searchInput,
                QueryDispatcherMock.Object,
                ContractConverterMock.Object,
                ErrorConverterMock.Object,
                default);
        }

        public override void SetupParameters()
        {
            _searchInput = new TestBuilder<string>().Create();
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterRecipeEndpoints();
        }

        public override void SetupQuery()
        {
            TestPropertyNotSetException.ThrowIfNull(_searchInput);

            Query = new SearchRecipesByNameQuery(_searchInput);
        }

        public override void SetupParametersForBadRequest()
        {
            _searchInput = string.Empty;
        }

        public override void SetupExpectedBadRequestMessage()
        {
            ExpectedBadRequestMessage = "Search input mustn't be null or empty";
        }
    }
}