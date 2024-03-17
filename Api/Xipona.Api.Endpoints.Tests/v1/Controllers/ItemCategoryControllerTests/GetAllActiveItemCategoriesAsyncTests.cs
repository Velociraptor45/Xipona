using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Queries.AllActiveItemCategories;
using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Shared;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ItemCategoryControllerTests;

public class GetAllActiveItemCategoriesAsyncTests : ControllerEnumerableQueryTestsBase<ItemCategoryController,
    AllActiveItemCategoriesQuery, ItemCategoryReadModel, ItemCategoryContract,
    GetAllActiveItemCategoriesAsyncTests.GetAllActiveItemCategoriesAsyncFixture>
{
    public GetAllActiveItemCategoriesAsyncTests() : base(new GetAllActiveItemCategoriesAsyncFixture())
    {
    }

    public sealed class GetAllActiveItemCategoriesAsyncFixture : ControllerEnumerableQueryFixtureBase
    {
        public GetAllActiveItemCategoriesAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override MethodInfo Method =>
            typeof(ItemCategoryController).GetMethod(nameof(ItemCategoryController.GetAllActiveItemCategoriesAsync))!;

        public override ItemCategoryController CreateSut()
        {
            return new ItemCategoryController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ItemCategoryController sut)
        {
            return await sut.GetAllActiveItemCategoriesAsync();
        }

        public override void SetupParameters()
        {
        }

        public override void SetupQuery()
        {
            Query = new AllActiveItemCategoriesQuery();
        }
    }
}