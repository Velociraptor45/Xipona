using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Xipona.Api.ApplicationServices.Items.Commands;
using Xipona.Api.Contracts.Common;
using Xipona.Api.Contracts.Items.Commands.UpdateItemPrice;
using Xipona.Api.Core.TestKit;
using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.TestKit.Common;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Endpoints.Tests.Common;
using Xipona.Api.Endpoints.Tests.Common.StatusResults;
using Xipona.Api.TestTools.Exceptions;
using System.Net.Http;

namespace Xipona.Api.Endpoints.Tests.v1.Endpoints.ItemTests;

public class UpdateItemPriceTests : EndpointCommandTestsBase<(Guid, UpdateItemPriceContract), UpdateItemPriceCommand, bool,
    UpdateItemPriceTests.UpdateItemPriceFixture>
{
    public UpdateItemPriceTests() : base(new UpdateItemPriceFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.ItemNotFound)]
    [InlineData(ErrorReasonCode.ItemTypeNotFound)]
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
        var unprocessableEntity = result as NotFound<ErrorContract>;
        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class UpdateItemPriceFixture : EndpointCommandFixtureBase
    {
        private UpdateItemPriceContract? _contract;
        private readonly Guid _itemId = Guid.NewGuid();

        public UpdateItemPriceFixture()
        {
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(new List<ErrorReasonCode>
            {
                ErrorReasonCode.ItemNotFound,
                ErrorReasonCode.ItemTypeNotFound
            }));
            PossibleResultsList.Add(new NoContentStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
        }

        public override string RoutePattern => "/v1/items/{id:guid}/update-price";
        public override HttpMethod HttpMethod => HttpMethod.Put;

        public override async Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);

            return await ItemEndpoints.UpdateItemPrice(_itemId, _contract,
                CommandDispatcherMock.Object,
                ErrorConverterMock.Object,
                CommandConverterMock.Object,
                TestContext.Current.CancellationToken);
        }

        public override void SetupParameters()
        {
            _contract = new TestBuilder<UpdateItemPriceContract>().Create();
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterItemEndpoints();
        }

        public override (Guid, UpdateItemPriceContract) GetCommandConverterInput()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            TestPropertyNotSetException.ThrowIfNull(Command);

            return (_itemId, _contract);
        }

        public override void SetupCommand()
        {
            Command = new DomainTestBuilder<UpdateItemPriceCommand>().Create();
        }
    }
}