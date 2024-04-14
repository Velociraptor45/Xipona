using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.Stores.Commands.DeleteStore;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.StoreControllerTests;

public class DeleteStoreAsyncTests : ControllerCommandTestsBase<StoreController,
    DeleteStoreCommand, bool, DeleteStoreAsyncTests.DeleteStoreAsyncFixture>
{
    public DeleteStoreAsyncTests() : base(new DeleteStoreAsyncFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.StoreNotFound)]
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

    public sealed class DeleteStoreAsyncFixture : ControllerCommandFixtureBase
    {
        private readonly Guid _storeId = Guid.NewGuid();

        public DeleteStoreAsyncFixture()
        {
            PossibleResultsList.Add(new NoContentStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(ErrorReasonCode.StoreNotFound));
        }

        public override System.Reflection.MethodInfo Method =>
            typeof(StoreController).GetMethod(nameof(StoreController.DeleteStoreAsync))!;

        public override StoreController CreateSut()
        {
            return new StoreController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override Task<IActionResult> ExecuteTestMethod(StoreController sut)
        {
            return sut.DeleteStoreAsync(_storeId);
        }

        public override void SetupParameters()
        {
        }

        public override void SetupCommand()
        {
            Command = new DeleteStoreCommand(new StoreId(_storeId));
        }
    }
}