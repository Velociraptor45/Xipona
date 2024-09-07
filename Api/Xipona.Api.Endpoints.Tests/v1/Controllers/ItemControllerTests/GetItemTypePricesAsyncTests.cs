using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.GetItemTypePrices;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.GetItemTypePrices;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using System.Reflection;
using static ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ItemControllerTests.GetItemTypePricesAsyncTests;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ItemControllerTests;

public class GetItemTypePricesAsyncTests() :
    ControllerQueryTestsBase<ItemController, GetItemTypePricesQuery, ItemTypePricesReadModel, ItemTypePricesContract,
        GetItemTypePricesAsyncFixture>(new GetItemTypePricesAsyncFixture())
{
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

    public sealed class GetItemTypePricesAsyncFixture : ControllerQueryFixtureBase
    {
        private readonly Guid _itemId = Guid.NewGuid();
        private readonly Guid _storeId = Guid.NewGuid();

        public GetItemTypePricesAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(ErrorReasonCode.ItemNotFound));
        }

        public override MethodInfo Method => typeof(ItemController).GetMethod(nameof(ItemController.GetItemTypePricesAsync))!;

        public override ItemController CreateSut()
        {
            return new ItemController(QueryDispatcherMock.Object, CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override Task<IActionResult> ExecuteTestMethod(ItemController sut)
        {
            return sut.GetItemTypePricesAsync(_itemId, _storeId);
        }

        public override void SetupParameters()
        {
        }

        public override void SetupQuery()
        {
            Query = new GetItemTypePricesQuery(new ItemId(_itemId), new StoreId(_storeId));
        }
    }
}