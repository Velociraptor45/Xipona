﻿using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Queries.ItemAmountsForOneServing;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.RecipeControllerTests;

public class GetItemAmountsForOneServingAsyncTests : ControllerQueryTestsBase<RecipeController, ItemAmountsForOneServingQuery,
    IEnumerable<ItemAmountForOneServing>, ItemAmountsForOneServingContract,
    GetItemAmountsForOneServingAsyncTests.GetItemAmountsForOneServingAsyncTestsFixture>
{
    public GetItemAmountsForOneServingAsyncTests() : base(new GetItemAmountsForOneServingAsyncTestsFixture())
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
        var sut = Fixture.CreateSut();

        // Act
        var result = await Fixture.ExecuteTestMethod(sut);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var unprocessableEntity = result as NotFoundObjectResult;
        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class GetItemAmountsForOneServingAsyncTestsFixture : ControllerQueryFixtureBase
    {
        private Guid? _recipeId;

        public GetItemAmountsForOneServingAsyncTestsFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(ErrorReasonCode.RecipeNotFound,
                ErrorReasonCode.ItemNotFound));
        }

        public override MethodInfo Method =>
            typeof(RecipeController).GetMethod(nameof(RecipeController.GetItemAmountsForOneServingAsync))!;

        public override RecipeController CreateSut()
        {
            return new RecipeController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override void SetupParameters()
        {
            _recipeId = Guid.NewGuid();
        }

        public override async Task<IActionResult> ExecuteTestMethod(RecipeController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_recipeId);
            return await sut.GetItemAmountsForOneServingAsync(_recipeId.Value);
        }

        public override void SetupQuery()
        {
            TestPropertyNotSetException.ThrowIfNull(_recipeId);

            Query = new ItemAmountsForOneServingQuery(new RecipeId(_recipeId.Value));
        }
    }
}