using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Queries.AllIngredientQuantityTypes;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries.Quantities;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.RecipeControllerTests;

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