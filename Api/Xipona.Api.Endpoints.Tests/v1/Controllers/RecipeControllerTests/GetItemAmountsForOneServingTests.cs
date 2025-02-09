using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Queries.ItemAmountsForOneServing;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.RecipeControllerTests;

public class GetItemAmountsForOneServingTests : EndpointQueryNoConverterTestsBase<ItemAmountsForOneServingQuery,
    IEnumerable<ItemAmountForOneServing>, ItemAmountsForOneServingContract,
    GetItemAmountsForOneServingTests.GetItemAmountsForOneServingTestsFixture>
{
    public GetItemAmountsForOneServingTests() : base(new GetItemAmountsForOneServingTestsFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.RecipeNotFound)]
    [InlineData(ErrorReasonCode.ItemNotFound)]
    public async Task EndpointCall_WithDomainException_ShouldReturnNotFound(ErrorReasonCode errorCode)
    {
        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupQuery();
        Fixture.SetupDomainException(errorCode);
        Fixture.SetupDomainExceptionInQueryDispatcher();
        Fixture.SetupExpectedErrorContract();
        Fixture.SetupErrorConversion();

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<NotFound<ErrorContract>>();
        var unprocessableEntity = result as NotFound<ErrorContract>;
        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class GetItemAmountsForOneServingTestsFixture : EndpointQueryNoConverterFixtureBase
    {
        private Guid? _recipeId;

        public GetItemAmountsForOneServingTestsFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(ErrorReasonCode.RecipeNotFound,
                ErrorReasonCode.ItemNotFound));
        }

        public override string RoutePattern => "/v1/recipes/{id:guid}/item-amounts-for-one-serving";

        public override void SetupParameters()
        {
            _recipeId = Guid.NewGuid();
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterRecipeEndpoints();
        }

        public override async Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_recipeId);
            return await RecipeEndpoints.GetItemAmountsForOneServing(
                _recipeId.Value,
                QueryDispatcherMock.Object,
                ContractConverterMock.Object,
                ErrorConverterMock.Object,
                default);
        }

        public override void SetupQuery()
        {
            TestPropertyNotSetException.ThrowIfNull(_recipeId);

            Query = new ItemAmountsForOneServingQuery(new RecipeId(_recipeId.Value));
        }
    }
}