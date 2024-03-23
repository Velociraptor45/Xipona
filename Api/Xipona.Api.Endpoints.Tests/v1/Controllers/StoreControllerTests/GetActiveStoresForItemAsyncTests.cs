using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.StoreControllerTests;

public class GetActiveStoresForItemAsyncTests : ControllerEnumerableQueryTestsBase<StoreController,
    GetActiveStoresForItemQuery, IStore, StoreForItemContract,
    GetActiveStoresForItemAsyncTests.GetActiveStoresForItemAsyncFixture>
{
    public GetActiveStoresForItemAsyncTests() : base(new GetActiveStoresForItemAsyncFixture())
    {
    }

    public sealed class GetActiveStoresForItemAsyncFixture : ControllerEnumerableQueryFixtureBase
    {
        public GetActiveStoresForItemAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override MethodInfo Method =>
            typeof(StoreController).GetMethod(nameof(StoreController.GetActiveStoresForItemAsync))!;

        public override StoreController CreateSut()
        {
            return new StoreController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(StoreController sut)
        {
            return await sut.GetActiveStoresForItemAsync();
        }

        public override void SetupParameters()
        {
        }

        public override void SetupQuery()
        {
            Query = new GetActiveStoresForItemQuery();
        }
    }
}