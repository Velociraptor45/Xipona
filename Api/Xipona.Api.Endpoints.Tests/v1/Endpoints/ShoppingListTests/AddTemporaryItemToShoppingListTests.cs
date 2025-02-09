using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Net.Http;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Endpoints.ShoppingListTests;

public class AddTemporaryItemToShoppingListTests : EndpointCommandWithReturnTypeTestsBase<
    (Guid, AddTemporaryItemToShoppingListContract), AddTemporaryItemToShoppingListCommand,
    TemporaryShoppingListItemReadModel, TemporaryShoppingListItemContract,
    AddTemporaryItemToShoppingListTests.AddTemporaryItemToShoppingListFixture>
{
    public AddTemporaryItemToShoppingListTests() : base(new AddTemporaryItemToShoppingListFixture())
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

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<NotFound<ErrorContract>>();
        var unprocessableEntity = result as NotFound<ErrorContract>;
        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class AddTemporaryItemToShoppingListFixture : EndpointCommandWithReturnTypeFixtureBase
    {
        private AddTemporaryItemToShoppingListContract? _contract;
        private readonly Guid _shoppingListId = Guid.NewGuid();

        public AddTemporaryItemToShoppingListFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new BadRequestStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(ErrorReasonCode.ShoppingListNotFound));
            PossibleResultsList.Add(new NotFoundStatusResult());
        }

        public override string RoutePattern => "/v1/shopping-lists/{id:guid}/items/temporary";
        public override HttpMethod HttpMethod => HttpMethod.Put;

        public override async Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);

            return await ShoppingListEndpoints.AddTemporaryItemToShoppingList(
                _shoppingListId, _contract,
                CommandDispatcherMock.Object,
                ErrorConverterMock.Object,
                CommandConverterMock.Object,
                ContractConverterMock.Object,
                default);
        }

        public override void SetupParameters()
        {
            _contract = new DomainTestBuilder<AddTemporaryItemToShoppingListContract>().Create();
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterShoppingListEndpoints();
        }

        public override (Guid, AddTemporaryItemToShoppingListContract) GetCommandConverterInput()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            return (_shoppingListId, _contract);
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