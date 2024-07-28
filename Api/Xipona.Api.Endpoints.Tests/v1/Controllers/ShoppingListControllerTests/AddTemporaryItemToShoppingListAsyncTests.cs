using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ShoppingListControllerTests;

public class AddTemporaryItemToShoppingListAsyncTests : ControllerCommandWithReturnTypeTestsBase<ShoppingListController,
    AddTemporaryItemToShoppingListCommand, TemporaryShoppingListItemReadModel, TemporaryShoppingListItemContract,
    AddTemporaryItemToShoppingListAsyncTests.AddTemporaryItemToShoppingListAsyncFixture>
{
    public AddTemporaryItemToShoppingListAsyncTests() : base(new AddTemporaryItemToShoppingListAsyncFixture())
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

    public sealed class AddTemporaryItemToShoppingListAsyncFixture : ControllerCommandWithReturnTypeFixtureBase
    {
        private AddTemporaryItemToShoppingListContract? _contract;
        private readonly Guid _shoppingListId = Guid.NewGuid();

        public AddTemporaryItemToShoppingListAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new BadRequestStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(ErrorReasonCode.ShoppingListNotFound));
            PossibleResultsList.Add(new NotFoundStatusResult());
        }

        public override MethodInfo Method =>
            typeof(ShoppingListController).GetMethod(nameof(ShoppingListController.AddTemporaryItemToShoppingListAsync))!;

        public override ShoppingListController CreateSut()
        {
            return new ShoppingListController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ShoppingListController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);

            return await sut.AddTemporaryItemToShoppingListAsync(_shoppingListId, _contract);
        }

        public override void SetupParameters()
        {
            _contract = new DomainTestBuilder<AddTemporaryItemToShoppingListContract>().Create();
        }

        public override void SetupCommand()
        {
            Command = new DomainTestBuilder<AddTemporaryItemToShoppingListCommand>().Create();
        }

        public override void SetupCommandConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            TestPropertyNotSetException.ThrowIfNull(Command);

            EndpointConvertersMock.SetupToDomain((_shoppingListId, _contract), Command);
        }

        public override void SetupParametersForBadRequest()
        {
            _contract = new DomainTestBuilder<AddTemporaryItemToShoppingListContract>()
                .FillPropertyWith(c => c.ItemName, string.Empty)
                .Create();
        }

        public override void SetupExpectedBadRequestMessage()
        {
            ExpectedBadRequestMessage = "Item name mustn't be empty";
        }
    }
}