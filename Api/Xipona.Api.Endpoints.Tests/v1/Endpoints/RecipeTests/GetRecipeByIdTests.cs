using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Xipona.Api.ApplicationServices.Recipes.Queries.RecipeById;
using Xipona.Api.Contracts.Common;
using Xipona.Api.Contracts.Recipes.Queries.Get;
using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Recipes.Models;
using Xipona.Api.Domain.Recipes.Services.Queries;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Endpoints.Tests.Common;
using Xipona.Api.Endpoints.Tests.Common.StatusResults;
using Xipona.Api.TestTools.Exceptions;

namespace Xipona.Api.Endpoints.Tests.v1.Endpoints.RecipeTests;

public class GetRecipeByIdTests : EndpointQueryNoConverterTestsBase<RecipeByIdQuery,
    RecipeReadModel, RecipeContract, GetRecipeByIdTests.GetRecipeByIdFixture>
{
    public GetRecipeByIdTests() : base(new GetRecipeByIdFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.RecipeNotFound)]
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
        var notFound = result as NotFound<ErrorContract>;
        notFound!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class GetRecipeByIdFixture : EndpointQueryNoConverterFixtureBase
    {
        private Guid? _recipeId;

        public GetRecipeByIdFixture()
        {
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(ErrorReasonCode.RecipeNotFound));
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
        }

        public override string RoutePattern => "/v1/recipes/{id:guid}";

        public override async Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_recipeId);

            return await RecipeEndpoints.GetRecipeById(
                _recipeId.Value,
                QueryDispatcherMock.Object,
                ContractConverterMock.Object,
                ErrorConverterMock.Object,
                TestContext.Current.CancellationToken);
        }

        public override void SetupParameters()
        {
            _recipeId = Guid.NewGuid();
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterRecipeEndpoints();
        }

        public override void SetupQuery()
        {
            TestPropertyNotSetException.ThrowIfNull(_recipeId);
            Query = new RecipeByIdQuery(new RecipeId(_recipeId.Value));
        }
    }
}