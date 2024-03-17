using ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Queries.SearchByTagIds;
using ProjectHermes.Xipona.Api.ApplicationServices.Tests.Common;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Tests.Recipes.Queries;

public class SearchRecipesByTagsQueryHandlerTests : QueryHandlerTestsBase<SearchRecipesByTagsQueryHandler,
    SearchRecipesByTagsQuery, IEnumerable<RecipeSearchResult>>
{
    public SearchRecipesByTagsQueryHandlerTests() : base(new SearchRecipesByTagIdsQueryHandlerFixture())
    {
    }

    private class SearchRecipesByTagIdsQueryHandlerFixture : IQueryHandlerBaseFixture
    {
        private readonly RecipeQueryServiceMock _serviceMock = new(MockBehavior.Strict);
        public SearchRecipesByTagsQuery? Query { get; private set; }
        public IEnumerable<RecipeSearchResult>? ExpectedResult { get; private set; }

        public SearchRecipesByTagsQueryHandler CreateSut()
        {
            return new SearchRecipesByTagsQueryHandler(_ => _serviceMock.Object);
        }

        public void Setup()
        {
            CreateQuery();
            CreateExpectedResult();
            SetupServiceReturningExpectedResult();
        }

        private void CreateQuery()
        {
            Query = new DomainTestBuilder<SearchRecipesByTagsQuery>().Create();
        }

        private void CreateExpectedResult()
        {
            ExpectedResult = new DomainTestBuilder<RecipeSearchResult>().CreateMany(2);
        }

        private void SetupServiceReturningExpectedResult()
        {
            TestPropertyNotSetException.ThrowIfNull(Query);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

            _serviceMock.SetupSearchByTagIdsAsync(Query.TagIds, ExpectedResult);
        }
    }
}