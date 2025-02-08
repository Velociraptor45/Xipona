using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.ItemById;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Endpoints.ItemTests;

public class GetItemByIdTests : EndpointQueryNoConverterTestsBase<ItemByIdQuery, ItemReadModel, ItemContract, GetItemByIdTests.GetItemByIdFixture>
{
    public GetItemByIdTests() : base(new GetItemByIdFixture())
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

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<NotFound<ErrorContract>>();
        var unprocessableEntity = result as NotFound<ErrorContract>;
        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class GetItemByIdFixture : EndpointQueryNoConverterFixtureBase
    {
        private readonly Guid _itemId = Guid.NewGuid();

        public GetItemByIdFixture()
        {
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(new List<ErrorReasonCode>
            {
                ErrorReasonCode.ItemNotFound
            }));
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
        }


        public override string RoutePattern => "/v1/items/{id:guid}";

        public override async Task<IResult> ExecuteTestMethod()
        {
            return await ItemEndpoints.GetItemById(
                _itemId,
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
            Query = new ItemByIdQuery(new ItemId(_itemId));
        }
    }
}