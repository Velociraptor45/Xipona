using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.ModifyManufacturer;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Manufacturers.Commands;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Net.Http;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Endpoints.ManufacturerTests;

public class ModifyManufacturerTests : EndpointCommandTestsBase<ModifyManufacturerContract,
    ModifyManufacturerCommand, bool, ModifyManufacturerTests.ModifyManufacturerFixture>
{
    public ModifyManufacturerTests() : base(new ModifyManufacturerFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.ManufacturerNotFound)]
    public async Task EndpointCall_WithDomainException_ShouldReturnNotFound(ErrorReasonCode errorCode)
    {
        // Arrange
        Fixture.SetupCommand();
        Fixture.SetupParameters();
        Fixture.SetupCommandConverter();
        Fixture.SetupDomainException(errorCode);
        Fixture.SetupDomainExceptionInCommandDispatcher();
        Fixture.SetupExpectedErrorContract();
        Fixture.SetupErrorConversion();

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<NotFound<ErrorContract>>();
        var unprocessableEntity = result as NotFound<ErrorContract>;
        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class ModifyManufacturerFixture : EndpointCommandFixtureBase
    {
        private ModifyManufacturerContract? _contract;

        public ModifyManufacturerFixture()
        {
            PossibleResultsList.Add(new NoContentStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(new List<ErrorReasonCode>()
            {
                ErrorReasonCode.ManufacturerNotFound
            }));
        }

        public override string RoutePattern => "/v1/manufacturers";
        public override HttpMethod HttpMethod => HttpMethod.Put;

        public override async Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            return await ManufacturerEndpoints.ModifyManufacturer(
                _contract,
                CommandDispatcherMock.Object,
                ErrorConverterMock.Object,
                CommandConverterMock.Object,
                default);
        }

        public override void SetupParameters()
        {
            _contract = new DomainTestBuilder<ModifyManufacturerContract>().Create();
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterManufacturerEndpoints();
        }

        public override ModifyManufacturerContract GetCommandConverterInput()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            return _contract;
        }
    }
}