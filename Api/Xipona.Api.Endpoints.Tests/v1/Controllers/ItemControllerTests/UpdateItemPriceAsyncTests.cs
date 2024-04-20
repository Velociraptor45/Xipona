using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItemPrice;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ItemControllerTests;

public class UpdateItemPriceAsyncTests : ControllerCommandTestsBase<ItemController, UpdateItemPriceCommand, bool,
    UpdateItemPriceAsyncTests.UpdateItemPriceAsyncFixture>
{
    public UpdateItemPriceAsyncTests() : base(new UpdateItemPriceAsyncFixture())
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
        var sut = Fixture.CreateSut();

        // Act
        var result = await Fixture.ExecuteTestMethod(sut);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var unprocessableEntity = result as NotFoundObjectResult;
        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class UpdateItemPriceAsyncFixture : ControllerCommandFixtureBase
    {
        private UpdateItemPriceContract? _contract;
        private readonly Guid _itemId = Guid.NewGuid();

        public UpdateItemPriceAsyncFixture()
        {
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(new List<ErrorReasonCode>
            {
                ErrorReasonCode.ItemNotFound,
                ErrorReasonCode.ItemTypeNotFound
            }));
            PossibleResultsList.Add(new NoContentStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
        }

        public override MethodInfo Method =>
            typeof(ItemController).GetMethod(nameof(ItemController.UpdateItemPriceAsync))!;

        public override ItemController CreateSut()
        {
            return new ItemController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ItemController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);

            return await sut.UpdateItemPriceAsync(_itemId, _contract);
        }

        public override void SetupParameters()
        {
            _contract = new TestBuilder<UpdateItemPriceContract>().Create();
        }

        public override void SetupCommand()
        {
            Command = new DomainTestBuilder<UpdateItemPriceCommand>().Create();
        }

        public override void SetupCommandConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            TestPropertyNotSetException.ThrowIfNull(Command);

            EndpointConvertersMock.SetupToDomain((_itemId, _contract), Command);
        }
    }
}