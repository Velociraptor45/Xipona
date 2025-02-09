using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Queries.AllIngredientQuantityTypes;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries.Quantities;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.RecipeControllerTests;

public class GetAllIngredientQuantityTypesTests : EndpointEnumerableQueryNoConverterTestsBase<
    AllIngredientQuantityTypesQuery, IngredientQuantityTypeReadModel, IngredientQuantityTypeContract,
    GetAllIngredientQuantityTypesTests.GetAllIngredientQuantityTypesFixture>
{
    public GetAllIngredientQuantityTypesTests() : base(new GetAllIngredientQuantityTypesFixture())
    {
    }

    public sealed class GetAllIngredientQuantityTypesFixture : EndpointEnumerableQueryNoConverterFixtureBase
    {
        public GetAllIngredientQuantityTypesFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override string RoutePattern => "/v1/recipes/ingredient-quantity-types";

        public override async Task<IResult> ExecuteTestMethod()
        {
            return await RecipeEndpoints.GetAllIngredientQuantityTypes(
                QueryDispatcherMock.Object,
                ContractConverterMock.Object,
                default);
        }

        public override void SetupParameters()
        {
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterRecipeEndpoints();
        }

        public override void SetupQuery()
        {
            Query = new AllIngredientQuantityTypesQuery();
        }
    }
}