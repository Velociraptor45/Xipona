using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Xipona.Api.ApplicationServices.Recipes.Queries.SearchByTagIds;
using Xipona.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using Xipona.Api.Core.TestKit;
using Xipona.Api.Domain.Recipes.Services.Queries;
using Xipona.Api.Domain.RecipeTags.Models;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Endpoints.Tests.Common;
using Xipona.Api.Endpoints.Tests.Common.StatusResults;
using Xipona.Api.TestTools.Exceptions;

namespace Xipona.Api.Endpoints.Tests.v1.Endpoints.RecipeTests;

public class SearchRecipesByTagsTests : EndpointEnumerableQueryNoConverterTestsBase<SearchRecipesByTagsQuery,
        RecipeSearchResult, RecipeSearchResultContract, SearchRecipesByTagsTests.SearchRecipesByTagsFixture>
{
    public SearchRecipesByTagsTests() : base(new SearchRecipesByTagsFixture())
    {
    }

    public sealed class SearchRecipesByTagsFixture : EndpointEnumerableQueryNoConverterFixtureBase
    {
        private Guid[]? _tagIds;

        public SearchRecipesByTagsFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override string RoutePattern => "/v1/recipes/search-by-tags";

        public override async Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_tagIds);
            return await RecipeEndpoints.SearchRecipesByTags(
                _tagIds,
                QueryDispatcherMock.Object,
                ContractConverterMock.Object,
                default);
        }

        public override void SetupParameters()
        {
            _tagIds = new TestBuilder<Guid>().CreateMany(2).ToArray();
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterRecipeEndpoints();
        }

        public override void SetupQuery()
        {
            TestPropertyNotSetException.ThrowIfNull(_tagIds);
            Query = new SearchRecipesByTagsQuery(_tagIds.Select(t => new RecipeTagId(t)));
        }
    }
}