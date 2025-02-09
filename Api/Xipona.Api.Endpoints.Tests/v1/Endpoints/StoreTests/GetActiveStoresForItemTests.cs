using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProjectHermes.Xipona.Api.ApplicationServices.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Endpoints.StoreTests;

public class GetActiveStoresForItemTests : EndpointEnumerableQueryNoConverterTestsBase<
    GetActiveStoresForItemQuery, IStore, StoreForItemContract,
    GetActiveStoresForItemTests.GetActiveStoresForItemFixture>
{
    public GetActiveStoresForItemTests() : base(new GetActiveStoresForItemFixture())
    {
    }

    public sealed class GetActiveStoresForItemFixture : EndpointEnumerableQueryNoConverterFixtureBase
    {
        public GetActiveStoresForItemFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override string RoutePattern => "/v1/stores/active-for-item";

        public override async Task<IResult> ExecuteTestMethod()
        {
            return await StoreEndpoints.GetActiveStoresForItem(
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
            Query = new GetActiveStoresForItemQuery();
        }
    }
}