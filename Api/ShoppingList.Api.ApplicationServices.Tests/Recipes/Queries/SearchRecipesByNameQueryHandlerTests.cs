using ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.Common;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Services.Queries;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.Recipes.Queries;

public class SearchRecipesByNameQueryHandlerTests : QueryHandlerTestsBase<SearchRecipesByNameQueryHandler,
    SearchRecipesByNameQuery, IEnumerable<RecipeSearchResult>>
{
    public SearchRecipesByNameQueryHandlerTests() : base(new SearchRecipesByNameQueryHandlerFixture())
    {
    }

    private class SearchRecipesByNameQueryHandlerFixture : IQueryHandlerBaseFixture
    {
        private readonly RecipeQueryServiceMock _serviceMock = new(MockBehavior.Strict);

        public SearchRecipesByNameQuery? Query { get; private set; }
        public IEnumerable<RecipeSearchResult>? ExpectedResult { get; private set; }

        public SearchRecipesByNameQueryHandler CreateSut()
        {
            return new SearchRecipesByNameQueryHandler(_ => _serviceMock.Object);
        }

        public void Setup()
        {
            CreateQuery();
            CreateExpectedResult();
            SetupServiceReturningExpectedResult();
        }

        private void CreateQuery()
        {
            Query = new DomainTestBuilder<SearchRecipesByNameQuery>().Create();
        }

        private void CreateExpectedResult()
        {
            ExpectedResult = new DomainTestBuilder<RecipeSearchResult>().CreateMany(2);
        }

        private void SetupServiceReturningExpectedResult()
        {
            TestPropertyNotSetException.ThrowIfNull(Query);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

            _serviceMock.SetupSearchByNameAsync(Query.SearchInput, ExpectedResult);
        }
    }
}