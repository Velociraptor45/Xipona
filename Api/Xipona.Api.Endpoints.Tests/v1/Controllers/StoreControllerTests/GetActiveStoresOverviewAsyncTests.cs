using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.Stores.Queries.GetActiveStoresOverview;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresOverview;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.StoreControllerTests;

public class GetActiveStoresOverviewAsyncTests : ControllerEnumerableQueryTestsBase<StoreController,
    GetActiveStoresOverviewQuery, IStore, StoreSearchResultContract,
    GetActiveStoresOverviewAsyncTests.GetActiveStoresOverviewAsyncFixture>
{
    public GetActiveStoresOverviewAsyncTests() : base(new GetActiveStoresOverviewAsyncFixture())
    {
    }

    public sealed class GetActiveStoresOverviewAsyncFixture : ControllerEnumerableQueryFixtureBase
    {
        public GetActiveStoresOverviewAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override MethodInfo Method =>
            typeof(StoreController).GetMethod(nameof(StoreController.GetActiveStoresOverviewAsync))!;

        public override StoreController CreateSut()
        {
            return new StoreController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(StoreController sut)
        {
            return await sut.GetActiveStoresOverviewAsync();
        }

        public override void SetupParameters()
        {
        }

        public override void SetupQuery()
        {
            Query = new GetActiveStoresOverviewQuery();
        }
    }
}