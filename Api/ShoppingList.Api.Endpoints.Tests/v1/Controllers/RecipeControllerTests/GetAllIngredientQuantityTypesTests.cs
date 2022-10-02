using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Queries.AllIngredientQuantityTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries.Quantities;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.RecipeControllerTests;

public class GetAllIngredientQuantityTypesTests : ControllerEnumerableQueryTestsBase<RecipeController,
    AllIngredientQuantityTypesQuery, IngredientQuantityTypeReadModel, IngredientQuantityTypeContract,
    GetAllIngredientQuantityTypesTests.GetAllIngredientQuantityTypesFixture>
{
    public GetAllIngredientQuantityTypesTests() : base(new GetAllIngredientQuantityTypesFixture())
    {
    }

    public sealed class GetAllIngredientQuantityTypesFixture : ControllerEnumerableQueryFixtureBase
    {
        public GetAllIngredientQuantityTypesFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override MethodInfo Method =>
            typeof(RecipeController).GetMethod(nameof(RecipeController.GetAllIngredientQuantityTypes))!;

        public override RecipeController CreateSut()
        {
            return new RecipeController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(RecipeController sut)
        {
            return await sut.GetAllIngredientQuantityTypes();
        }

        public override void SetupParameters()
        {
        }

        public override void SetupQuery()
        {
            Query = new AllIngredientQuantityTypesQuery();
        }
    }
}