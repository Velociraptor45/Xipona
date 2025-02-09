using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProjectHermes.Xipona.Api.ApplicationServices.Stores.Queries.GetActiveStoresOverview;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresOverview;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Endpoints.StoreTests;

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
                default);
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