using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.RecipeControllerTests;

public class SearchRecipesByNameAsyncTests : ControllerEnumerableQueryTestsBase<RecipeController, SearchRecipesByNameQuery,
    RecipeSearchResult, RecipeSearchResultContract, SearchRecipesByNameAsyncTests.SearchRecipesByNameAsyncFixture>
{
    public SearchRecipesByNameAsyncTests() : base(new SearchRecipesByNameAsyncFixture())
    {
    }

    public sealed class SearchRecipesByNameAsyncFixture : ControllerEnumerableQueryFixtureBase
    {
        private string? _searchInput;

        public SearchRecipesByNameAsyncFixture()
        {
            PossibleResultsList.Add(new UnprocessableEntityStatusResult());
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
            PossibleResultsList.Add(new BadRequestStatusResult());
        }

        public override MethodInfo Method =>
            typeof(RecipeController).GetMethod(nameof(RecipeController.SearchRecipesByNameAsync))!;

        public override RecipeController CreateSut()
        {
            return new RecipeController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(RecipeController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_searchInput);

            return await sut.SearchRecipesByNameAsync(_searchInput);
        }

        public override void SetupParameters()
        {
            _searchInput = new TestBuilder<string>().Create();
        }

        public override void SetupQuery()
        {
            TestPropertyNotSetException.ThrowIfNull(_searchInput);

            Query = new SearchRecipesByNameQuery(_searchInput);
        }

        public override void SetupParametersForBadRequest()
        {
            _searchInput = string.Empty;
        }

        public override void SetupExpectedBadRequestMessage()
        {
            ExpectedBadRequestMessage = "Search input mustn't be null or empty";
        }
    }
}