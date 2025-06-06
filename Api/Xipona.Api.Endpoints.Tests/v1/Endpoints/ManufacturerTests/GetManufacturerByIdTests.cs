using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Xipona.Api.ApplicationServices.Manufacturers.Queries.ManufacturerById;
using Xipona.Api.Contracts.Common;
using Xipona.Api.Contracts.Common.Queries;
using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Endpoints.Tests.Common;
using Xipona.Api.Endpoints.Tests.Common.StatusResults;

namespace Xipona.Api.Endpoints.Tests.v1.Endpoints.ManufacturerTests;

public class GetManufacturerByIdTests : EndpointQueryNoConverterTestsBase<
    ManufacturerByIdQuery, IManufacturer, ManufacturerContract,
    GetManufacturerByIdTests.GetManufacturerByIdFixture>
{
    public GetManufacturerByIdTests() : base(new GetManufacturerByIdFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.ManufacturerNotFound)]
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

    public sealed class GetManufacturerByIdFixture : EndpointQueryNoConverterFixtureBase
    {
        public GetManufacturerByIdFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(new List<ErrorReasonCode>()
            {
                ErrorReasonCode.ManufacturerNotFound
            }));
        }

        private readonly Guid _itemCategoryId = Guid.NewGuid();

        public override string RoutePattern => "/v1/manufacturers/{id:guid}";

        public override async Task<IResult> ExecuteTestMethod()
        {
            return await ManufacturerEndpoints.GetManufacturerById(_itemCategoryId,
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
            app.RegisterManufacturerEndpoints();
        }

        public override void SetupQuery()
        {
            Query = new ManufacturerByIdQuery(new ManufacturerId(_itemCategoryId));
        }
    }
}