using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemsToShoppingLists;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.ShoppingListControllerTests;

public class AddItemsToShoppingListsAsyncTests : ControllerCommandTestsBase<ShoppingListController,
    AddItemsToShoppingListsCommand, bool, AddItemsToShoppingListsAsyncTests.AddItemsToShoppingListsAsyncFixture>
{
    public AddItemsToShoppingListsAsyncTests() : base(new AddItemsToShoppingListsAsyncFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.StoreNotFound)]
    [InlineData(ErrorReasonCode.ItemNotFound)]
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

    public sealed class AddItemsToShoppingListsAsyncFixture : ControllerCommandFixtureBase
    {
        private AddItemsToShoppingListsContract? _contract;

        public AddItemsToShoppingListsAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(ErrorReasonCode.StoreNotFound,
                ErrorReasonCode.ItemNotFound));
            PossibleResultsList.Add(new NotFoundStatusResult());
        }

        public override MethodInfo Method =>
            typeof(ShoppingListController).GetMethod(nameof(ShoppingListController.AddItemsToShoppingListsAsync))!;

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

            return await sut.AddItemsToShoppingListsAsync(_contract);
        }

        public override void SetupParameters()
        {
            _contract = new DomainTestBuilder<AddItemsToShoppingListsContract>().Create();
        }

        public override void SetupCommand()
        {
            Command = new DomainTestBuilder<AddItemsToShoppingListsCommand>().Create();
        }

        public override void SetupCommandConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            TestPropertyNotSetException.ThrowIfNull(Command);

            EndpointConvertersMock.SetupToDomain(_contract, Command);
        }
    }
}