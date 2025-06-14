using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Xipona.Api.ApplicationServices.Recipes.Commands.ModifyRecipe;
using Xipona.Api.Contracts.Common;
using Xipona.Api.Contracts.Recipes.Commands.ModifyRecipe;
using Xipona.Api.Core.TestKit;
using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Endpoints.Tests.Common;
using Xipona.Api.Endpoints.Tests.Common.StatusResults;
using Xipona.Api.TestTools.Exceptions;
using System.Net.Http;

namespace Xipona.Api.Endpoints.Tests.v1.Endpoints.RecipeTests;

public class ModifyRecipeTests : EndpointCommandTestsBase<(Guid, ModifyRecipeContract), ModifyRecipeCommand, bool,
        ModifyRecipeTests.ModifyRecipeFixture>
{
    public ModifyRecipeTests() : base(new ModifyRecipeFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.RecipeNotFound)]
    [InlineData(ErrorReasonCode.IngredientNotFound)]
    [InlineData(ErrorReasonCode.PreparationStepNotFound)]
    public async Task EndpointCall_WithDomainException_ShouldReturnNotFound(ErrorReasonCode errorCode)
    {
        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupCommand();
        Fixture.SetupCommandConverter();
        Fixture.SetupDomainException(errorCode);
        Fixture.SetupDomainExceptionInCommandDispatcher();
        Fixture.SetupExpectedErrorContract();
        Fixture.SetupErrorConversion();

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<NotFound<ErrorContract>>();
        var notFound = result as NotFound<ErrorContract>;
        notFound!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class ModifyRecipeFixture : EndpointCommandFixtureBase
    {
        private ModifyRecipeContract? _contract;
        private readonly Guid _recipeId = Guid.NewGuid();

        public ModifyRecipeFixture()
        {
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(
                ErrorReasonCode.RecipeNotFound,
                ErrorReasonCode.IngredientNotFound,
                ErrorReasonCode.PreparationStepNotFound));
            PossibleResultsList.Add(new NoContentStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
        }

        public override string RoutePattern => "/v1/recipes/{id:guid}/modify";
        public override HttpMethod HttpMethod => HttpMethod.Put;

        public override async Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            return await RecipeEndpoints.ModifyRecipe(
                _recipeId,
                _contract,
                CommandDispatcherMock.Object,
                CommandConverterMock.Object,
                ErrorConverterMock.Object,
                TestContext.Current.CancellationToken);
        }

        public override void SetupParameters()
        {
            _contract = new TestBuilder<ModifyRecipeContract>().Create();
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterRecipeEndpoints();
        }

        public override (Guid, ModifyRecipeContract) GetCommandConverterInput()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            return (_recipeId, _contract);
        }
    }
}