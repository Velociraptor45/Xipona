using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.RemoveItemDiscount;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemDiscount;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ShoppingListControllerTests;
public class RemoveItemDiscountAsyncTests : ControllerCommandTestsBase<ShoppingListController,
    RemoveItemDiscountCommand, bool, RemoveItemDiscountAsyncTests.RemoveItemDiscountAsyncFixture>
{
    public RemoveItemDiscountAsyncTests() : base(new RemoveItemDiscountAsyncFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.ShoppingListNotFound)]
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

    public sealed class RemoveItemDiscountAsyncFixture : ControllerCommandFixtureBase
    {
        private readonly Guid _shoppingListId = Guid.NewGuid();
        private RemoveItemDiscountContract? _contract;

        public RemoveItemDiscountAsyncFixture()
        {
            PossibleResultsList.Remove(new NoContentStatusResult());
            PossibleResultsList.Remove(new NotFoundStatusResult());
            PossibleResultsList.Remove(new UnprocessableEntityStatusResult(ErrorReasonCode.ShoppingListNotFound));
        }

        public override MethodInfo Method =>
            typeof(ShoppingListController).GetMethod(nameof(ShoppingListController.RemoveItemDiscountAsync))!;

        public override ShoppingListController CreateSut()
        {
            return new ShoppingListController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override Task<IActionResult> ExecuteTestMethod(ShoppingListController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);

            return sut.RemoveItemDiscountAsync(_shoppingListId, _contract);
        }

        public override void SetupParameters()
        {
            _contract = new DomainTestBuilder<RemoveItemDiscountContract>().Create();
        }

        public override void SetupCommand()
        {
            Command = new DomainTestBuilder<RemoveItemDiscountCommand>().Create();
        }

        public override void SetupCommandConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);

            EndpointConvertersMock.SetupToDomain((_shoppingListId, _contract), Command);
        }
    }
}
