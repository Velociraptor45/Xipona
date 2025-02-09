using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Commands.ModifyRecipe;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Commands.ModifyRecipe;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Net.Http;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.RecipeControllerTests;

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
                default);
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