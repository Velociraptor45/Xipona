using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Xipona.Api.ApplicationServices.Stores.Queries.GetActiveStoresForShopping;
using Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForShopping;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Endpoints.Tests.Common;
using Xipona.Api.Endpoints.Tests.Common.StatusResults;

namespace Xipona.Api.Endpoints.Tests.v1.Endpoints.StoreTests;

public class GetActiveStoresForShoppingTests : EndpointEnumerableQueryNoConverterTestsBase<
    GetActiveStoresForShoppingQuery, IStore, StoreForShoppingContract,
    GetActiveStoresForShoppingTests.GetActiveStoresForShoppingFixture>
{
    public GetActiveStoresForShoppingTests() : base(new GetActiveStoresForShoppingFixture())
    {
    }

    public sealed class GetActiveStoresForShoppingFixture : EndpointEnumerableQueryNoConverterFixtureBase
    {
        public GetActiveStoresForShoppingFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override string RoutePattern => "/v1/stores/active-for-shopping";

        public override async Task<IResult> ExecuteTestMethod()
        {
            return await StoreEndpoints.GetActiveStoresForShopping(
                QueryDispatcherMock.Object,
                ContractConverterMock.Object,
                TestContext.Current.CancellationToken);
        }

        public override void SetupParameters()
        {
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterStoreEndpoints();
        }

        public override void SetupQuery()
        {
            Query = new GetActiveStoresForShoppingQuery();
        }
    }
}