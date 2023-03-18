using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Queries.StoreById;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.Get;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.StoreControllerTests;

public class GetStoreByIdAsyncTests : ControllerQueryTestsBase<StoreController, GetStoreByIdQuery, IStore, StoreContract,
    GetStoreByIdAsyncTests.GetStoreByIdAsyncFixture>
{
    public GetStoreByIdAsyncTests() : base(new GetStoreByIdAsyncFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.StoreNotFound)]
    public async Task EndpointCall_WithDomainException_ShouldReturnNotFound(ErrorReasonCode errorCode)
    {
        // Arrange
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

    public sealed class GetStoreByIdAsyncFixture : ControllerQueryFixtureBase
    {
        private readonly Guid _storeId = Guid.NewGuid();

        public GetStoreByIdAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(
                new NotFoundStatusResult(new List<ErrorReasonCode> { ErrorReasonCode.StoreNotFound }));
        }

        public override MethodInfo Method =>
            typeof(StoreController).GetMethod(nameof(StoreController.GetStoreByIdAsync))!;

        public override StoreController CreateSut()
        {
            return new StoreController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(StoreController sut)
        {
            return await sut.GetStoreByIdAsync(_storeId);
        }

        public override void SetupParameters()
        {
        }

        public override void SetupQuery()
        {
            Query = new GetStoreByIdQuery(new StoreId(_storeId));
        }
    }
}