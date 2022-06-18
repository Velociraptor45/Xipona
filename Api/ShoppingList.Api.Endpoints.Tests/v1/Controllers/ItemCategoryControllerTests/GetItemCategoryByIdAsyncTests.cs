using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Queries.ItemCategoryById;
using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.ItemCategoryControllerTests;

public class GetItemCategoryByIdAsyncTests : ControllerQueryTestsBase<ItemCategoryController,
    ItemCategoryByIdQuery, IItemCategory, ItemCategoryContract,
    GetItemCategoryByIdAsyncTests.GetItemCategoryByIdAsyncFixture>
{
    public GetItemCategoryByIdAsyncTests() : base(new GetItemCategoryByIdAsyncFixture())
    {
    }

    public sealed class GetItemCategoryByIdAsyncFixture : ControllerQueryFixtureBase
    {
        public GetItemCategoryByIdAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(new List<ErrorReasonCode>()
            {
                ErrorReasonCode.ItemCategoryNotFound
            }));
        }

        private readonly Guid _itemCategoryId = Guid.NewGuid();

        public override MethodInfo Method =>
            typeof(ItemCategoryController).GetMethod(nameof(ItemCategoryController.GetItemCategoryByIdAsync))!;

        public override ItemCategoryController CreateSut()
        {
            return new ItemCategoryController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ItemCategoryController sut)
        {
            return await sut.GetItemCategoryByIdAsync(_itemCategoryId);
        }

        public override void SetupParameters()
        {
        }

        public override void SetupQuery()
        {
            Query = new ItemCategoryByIdQuery(new ItemCategoryId(_itemCategoryId));
        }
    }
}