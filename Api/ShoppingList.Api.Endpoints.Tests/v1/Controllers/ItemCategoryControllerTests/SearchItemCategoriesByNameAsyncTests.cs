using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Queries.ItemCategorySearch;
using ProjectHermes.ShoppingList.Api.Contracts.ItemCategories.Queries;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Queries;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.ItemCategoryControllerTests;

public class SearchItemCategoriesByNameAsyncTests : ControllerEnumerableQueryTestsBase<ItemCategoryController,
    ItemCategorySearchQuery, ItemCategorySearchResultReadModel, ItemCategorySearchResultContract,
    SearchItemCategoriesByNameAsyncTests.SearchItemCategoriesByNameAsyncFixture>
{
    public SearchItemCategoriesByNameAsyncTests() : base(new SearchItemCategoriesByNameAsyncFixture())
    {
    }

    public sealed class SearchItemCategoriesByNameAsyncFixture : ControllerEnumerableQueryFixtureBase
    {
        private bool? _includeDeleted;
        private string? _searchInput;

        public SearchItemCategoriesByNameAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
            PossibleResultsList.Add(new BadRequestStatusResult());
        }

        public override MethodInfo Method =>
            typeof(ItemCategoryController).GetMethod(nameof(ItemCategoryController.SearchItemCategoriesByNameAsync))!;

        public override ItemCategoryController CreateSut()
        {
            return new ItemCategoryController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ItemCategoryController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_searchInput);
            TestPropertyNotSetException.ThrowIfNull(_includeDeleted);
            return await sut.SearchItemCategoriesByNameAsync(_searchInput, _includeDeleted.Value);
        }

        public override void SetupParameters()
        {
            _searchInput = new TestBuilder<string>().Create();
            _includeDeleted = new TestBuilder<bool>().Create();
        }

        public override void SetupParametersForBadRequest()
        {
            _searchInput = string.Empty;
            _includeDeleted = new TestBuilder<bool>().Create();
        }

        public override void SetupQuery()
        {
            TestPropertyNotSetException.ThrowIfNull(_searchInput);
            TestPropertyNotSetException.ThrowIfNull(_includeDeleted);
            Query = new ItemCategorySearchQuery(_searchInput, _includeDeleted.Value);
        }

        public override void SetupExpectedBadRequestMessage()
        {
            ExpectedBadRequestMessage = "Search input mustn't be null or empty";
        }
    }
}