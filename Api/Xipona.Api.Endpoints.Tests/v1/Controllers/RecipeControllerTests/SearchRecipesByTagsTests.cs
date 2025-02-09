using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Queries.SearchByTagIds;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.RecipeControllerTests;

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