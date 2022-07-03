using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.ItemById;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.ItemControllerTests;

public class GetAsyncTests :
    ControllerQueryTestsBase<ItemController, ItemByIdQuery, StoreItemReadModel, StoreItemContract,
    GetAsyncTests.GetAsyncFixture>
{
    public GetAsyncTests() : base(new GetAsyncFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.ItemNotFound)]
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

    public sealed class GetAsyncFixture : ControllerQueryFixtureBase
    {
        private readonly Guid _itemId = Guid.NewGuid();

        public GetAsyncFixture()
        {
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(new List<ErrorReasonCode>
            {
                ErrorReasonCode.ItemNotFound
            }));
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
        }

        public override MethodInfo Method => typeof(ItemController).GetMethod(nameof(ItemController.GetAsync))!;

        public override ItemController CreateSut()
        {
            return new ItemController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ItemController sut)
        {
            return await sut.GetAsync(_itemId);
        }

        public override void SetupParameters()
        {
        }

        public override void SetupQuery()
        {
            Query = new ItemByIdQuery(new ItemId(_itemId));
        }
    }
}