using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Queries.ItemCategorySearch;
using ProjectHermes.Xipona.Api.ApplicationServices.Tests.Common;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Services.Queries;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Tests.ItemCategories.Queries.ItemCategorySearch;

public class ItemCategorySearchQueryHandlerTests : QueryHandlerTestsBase<ItemCategorySearchQueryHandler, ItemCategorySearchQuery,
    IEnumerable<ItemCategorySearchResultReadModel>>
{
    public ItemCategorySearchQueryHandlerTests() : base(new ItemCategorySearchQueryHandlerFixture())
    {
    }

    private sealed class ItemCategorySearchQueryHandlerFixture : IQueryHandlerBaseFixture
    {
        private readonly ItemCategoryQueryServiceMock _serviceMock = new(MockBehavior.Strict);

        public ItemCategorySearchQuery? Query { get; private set; }
        public IEnumerable<ItemCategorySearchResultReadModel>? ExpectedResult { get; private set; }

        public ItemCategorySearchQueryHandler CreateSut()
        {
            return new ItemCategorySearchQueryHandler(_ => _serviceMock.Object);
        }

        public void Setup()
        {
            CreateQuery();
            CreateExpectedResult();
            SetupServiceReturningExpectedResult();
        }

        private void CreateQuery()
        {
            Query = new DomainTestBuilder<ItemCategorySearchQuery>().Create();
        }

        private void CreateExpectedResult()
        {
            ExpectedResult = new DomainTestBuilder<ItemCategorySearchResultReadModel>().CreateMany(2);
        }

        private void SetupServiceReturningExpectedResult()
        {
            TestPropertyNotSetException.ThrowIfNull(Query);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

            _serviceMock.SetupGetAsync(Query.SearchInput, Query.IncludeDeleted, ExpectedResult);
        }
    }
}