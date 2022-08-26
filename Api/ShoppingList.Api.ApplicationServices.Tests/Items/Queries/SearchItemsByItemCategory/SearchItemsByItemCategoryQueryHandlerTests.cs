using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Queries.SearchItemsByItemCategory;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.Common;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Searches;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.Items.Queries.SearchItemsByItemCategory;

public class SearchItemsByItemCategoryQueryHandlerTests : QueryHandlerTestsBase<SearchItemsByItemCategoryQueryHandler,
    SearchItemsByItemCategoryQuery, IEnumerable<SearchItemByItemCategoryResult>>
{
    public SearchItemsByItemCategoryQueryHandlerTests() : base(new SearchItemsByItemCategoryQueryHandlerFixture())
    {
    }

    private sealed class SearchItemsByItemCategoryQueryHandlerFixture : IQueryHandlerBaseFixture
    {
        private readonly ItemSearchServiceMock _serviceMock = new(MockBehavior.Strict);

        public SearchItemsByItemCategoryQuery? Query { get; private set; }
        public IEnumerable<SearchItemByItemCategoryResult>? ExpectedResult { get; private set; }

        public SearchItemsByItemCategoryQueryHandler CreateSut()
        {
            return new SearchItemsByItemCategoryQueryHandler(_ => _serviceMock.Object);
        }

        public void Setup()
        {
            CreateQuery();
            CreateExpectedResult();
            SetupServiceReturningExpectedResult();
        }

        private void CreateQuery()
        {
            Query = new DomainTestBuilder<SearchItemsByItemCategoryQuery>().Create();
        }

        private void CreateExpectedResult()
        {
            ExpectedResult = new DomainTestBuilder<SearchItemByItemCategoryResult>().CreateMany(2);
        }

        private void SetupServiceReturningExpectedResult()
        {
            TestPropertyNotSetException.ThrowIfNull(Query);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

            _serviceMock.SetupSearchAsync(Query.ItemCategoryId, ExpectedResult);
        }
    }
}