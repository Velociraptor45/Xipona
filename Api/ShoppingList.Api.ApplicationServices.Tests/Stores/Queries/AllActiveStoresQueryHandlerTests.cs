using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.Common;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Services.Queries;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.Stores.Queries;
public class AllActiveStoresQueryHandlerTests : QueryHandlerTestsBase<AllActiveStoresQueryHandler,
    AllActiveStoresQuery, IEnumerable<StoreReadModel>>
{
    public AllActiveStoresQueryHandlerTests() : base(new AllActiveStoresQueryHandlerFixture())
    {
    }

    private sealed class AllActiveStoresQueryHandlerFixture : IQueryHandlerBaseFixture
    {
        private readonly StoreQueryServiceMock _serviceMock = new(MockBehavior.Strict);

        public AllActiveStoresQuery? Query { get; private set; }
        public IEnumerable<StoreReadModel>? ExpectedResult { get; private set; }
        public AllActiveStoresQueryHandler CreateSut()
        {
            return new AllActiveStoresQueryHandler(_ => _serviceMock.Object);
        }

        public void Setup()
        {
            CreateQuery();
            CreateExpectedResult();
            SetupServiceReturningExpectedResult();
        }

        private void CreateQuery()
        {
            Query = new DomainTestBuilder<AllActiveStoresQuery>().Create();
        }

        private void CreateExpectedResult()
        {
            ExpectedResult = new DomainTestBuilder<StoreReadModel>().CreateMany(2);
        }

        private void SetupServiceReturningExpectedResult()
        {
            TestPropertyNotSetException.ThrowIfNull(Query);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

            _serviceMock.SetupGetActiveAsync(ExpectedResult);
        }
    }
}
