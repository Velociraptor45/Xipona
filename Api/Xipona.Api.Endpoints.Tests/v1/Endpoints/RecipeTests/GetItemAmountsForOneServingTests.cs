using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Xipona.Api.ApplicationServices.Recipes.Queries.ItemAmountsForOneServing;
using Xipona.Api.Contracts.Common;
using Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing;
using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Recipes.Models;
using Xipona.Api.Domain.Recipes.Services.Queries;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Endpoints.Tests.Common;
using Xipona.Api.Endpoints.Tests.Common.StatusResults;
using Xipona.Api.TestTools.Exceptions;

namespace Xipona.Api.Endpoints.Tests.v1.Endpoints.RecipeTests;

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
                TestContext.Current.CancellationToken);
        }

        public override void SetupQuery()
        {
            TestPropertyNotSetException.ThrowIfNull(_recipeId);

            Query = new ItemAmountsForOneServingQuery(new RecipeId(_recipeId.Value));
        }
    }
}