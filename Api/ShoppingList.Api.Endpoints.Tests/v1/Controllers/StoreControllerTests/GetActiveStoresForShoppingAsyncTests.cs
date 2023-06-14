using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Queries.GetActiveStoresForShopping;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresForShopping;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.StoreControllerTests;

public class GetActiveStoresForShoppingAsyncTests : ControllerEnumerableQueryTestsBase<StoreController,
    GetActiveStoresForShoppingQuery, IStore, StoreForShoppingContract,
    GetActiveStoresForShoppingAsyncTests.GetActiveStoresForShoppingAsyncFixture>
{
    public GetActiveStoresForShoppingAsyncTests() : base(new GetActiveStoresForShoppingAsyncFixture())
    {
    }

    public sealed class GetActiveStoresForShoppingAsyncFixture : ControllerEnumerableQueryFixtureBase
    {
        public GetActiveStoresForShoppingAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override MethodInfo Method =>
            typeof(StoreController).GetMethod(nameof(StoreController.GetActiveStoresForShoppingAsync))!;

        public override StoreController CreateSut()
        {
            return new StoreController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(StoreController sut)
        {
            return await sut.GetActiveStoresForShoppingAsync();
        }

        public override void SetupParameters()
        {
        }

        public override void SetupQuery()
        {
            Query = new GetActiveStoresForShoppingQuery();
        }
    }
}