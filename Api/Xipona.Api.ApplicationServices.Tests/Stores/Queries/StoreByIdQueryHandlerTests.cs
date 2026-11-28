using Xipona.Api.ApplicationServices.Stores.Queries.StoreById;
using Xipona.Api.ApplicationServices.Tests.Common;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.TestKit.Common;
using Xipona.Api.Domain.TestKit.Stores.Models;
using Xipona.Api.Domain.TestKit.Stores.Services.Queries;
using Xipona.Api.TestTools.Exceptions;

namespace Xipona.Api.ApplicationServices.Tests.Stores.Queries;

public class StoreByIdQueryHandlerTests : QueryHandlerTestsBase<GetStoreByIdQueryHandler, GetStoreByIdQuery, IStore>
{
    public StoreByIdQueryHandlerTests() : base(new StoreByIdQueryHandlerFixture())
    {
    }

    private sealed class StoreByIdQueryHandlerFixture : IQueryHandlerBaseFixture
    {
        private readonly StoreQueryServiceMock _serviceMock = new(MockBehavior.Strict);
        public GetStoreByIdQuery? Query { get; private set; }
        public IStore? ExpectedResult { get; private set; }

        public GetStoreByIdQueryHandler CreateSut()
        {
            return new GetStoreByIdQueryHandler(_ => _serviceMock.Object);
        }

        public void Setup()
        {
            CreateQuery();
            CreateExpectedResult();
            SetupServiceReturningExpectedResult();
        }

        private void CreateQuery()
        {
            Query = new DomainTestBuilder<GetStoreByIdQuery>().Create();
        }

        private void CreateExpectedResult()
        {
            ExpectedResult = new StoreBuilder().Create();
        }

        private void SetupServiceReturningExpectedResult()
        {
            TestPropertyNotSetException.ThrowIfNull(Query);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

            _serviceMock.SetupGetActiveAsync(Query.StoreId, ExpectedResult);
        }
    }
}