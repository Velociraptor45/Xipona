using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.GetItemTypePrices;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.GetItemTypePrices;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using static ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ItemControllerTests.GetItemTypePricesTests;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ItemControllerTests;

public class GetItemTypePricesTests() :
    EndpointQueryNoConverterTestsBase<GetItemTypePricesQuery, ItemTypePricesReadModel, ItemTypePricesContract,
        GetItemTypePricesFixture>(new GetItemTypePricesFixture())
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

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<NotFound<ErrorContract>>();
        var unprocessableEntity = result as NotFound<ErrorContract>;
        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class GetItemTypePricesFixture : EndpointQueryNoConverterFixtureBase
    {
        private readonly Guid _itemId = Guid.NewGuid();
        private readonly Guid _storeId = Guid.NewGuid();

        public GetItemTypePricesFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(ErrorReasonCode.ItemNotFound));
        }

        public override string RoutePattern => "/v1/items/{id:guid}/type-prices";

        public override Task<IResult> ExecuteTestMethod()
        {
            return ItemEndpoints.GetItemTypePrices(_itemId, _storeId,
                QueryDispatcherMock.Object,
                ContractConverterMock.Object,
                ErrorConverterMock.Object,
                default);
        }

        public override void SetupParameters()
        {
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterItemEndpoints();
        }

        public override void SetupQuery()
        {
            Query = new GetItemTypePricesQuery(new ItemId(_itemId), new StoreId(_storeId));
        }
    }
}