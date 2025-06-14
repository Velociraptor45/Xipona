using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Xipona.Api.ApplicationServices.Stores.Queries.GetActiveStoresOverview;
using Xipona.Api.Contracts.Stores.Queries.GetActiveStoresOverview;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Endpoints.Tests.Common;
using Xipona.Api.Endpoints.Tests.Common.StatusResults;

namespace Xipona.Api.Endpoints.Tests.v1.Endpoints.StoreTests;

public class GetActiveStoresOverviewTests : EndpointEnumerableQueryNoConverterTestsBase<
    GetActiveStoresOverviewQuery, IStore, StoreSearchResultContract,
    GetActiveStoresOverviewTests.GetActiveStoresOverviewFixture>
{
    public GetActiveStoresOverviewTests() : base(new GetActiveStoresOverviewFixture())
    {
    }

    public sealed class GetActiveStoresOverviewFixture : EndpointEnumerableQueryNoConverterFixtureBase
    {
        public GetActiveStoresOverviewFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override string RoutePattern => "/v1/stores/active-overview";

        public override async Task<IResult> ExecuteTestMethod()
        {
            return await StoreEndpoints.GetActiveStoresOverview(
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
            Query = new GetActiveStoresOverviewQuery();
        }
    }
}