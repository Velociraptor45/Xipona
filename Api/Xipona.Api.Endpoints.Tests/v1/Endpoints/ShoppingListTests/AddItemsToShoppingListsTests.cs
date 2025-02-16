using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemsToShoppingLists;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Net.Http;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Endpoints.ShoppingListTests;

public class AddItemsToShoppingListsTests : EndpointCommandTestsBase<AddItemsToShoppingListsContract,
    AddItemsToShoppingListsCommand, bool, AddItemsToShoppingListsTests.AddItemsToShoppingListsFixture>
{
    public AddItemsToShoppingListsTests() : base(new AddItemsToShoppingListsFixture())
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

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<NotFound<ErrorContract>>();
        var unprocessableEntity = result as NotFound<ErrorContract>;
        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class AddItemsToShoppingListsFixture : EndpointCommandFixtureBase
    {
        private AddItemsToShoppingListsContract? _contract;

        public AddItemsToShoppingListsFixture()
        {
            PossibleResultsList.Add(new NoContentStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(ErrorReasonCode.StoreNotFound,
                ErrorReasonCode.ItemNotFound));
            PossibleResultsList.Add(new NotFoundStatusResult());
        }

        public override string RoutePattern => "/v1/shopping-lists/add-items-to-shopping-lists";
        public override HttpMethod HttpMethod => HttpMethod.Put;

        public override async Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);

            return await ShoppingListEndpoints.AddItemsToShoppingLists(
                _contract,
                CommandDispatcherMock.Object,
                ErrorConverterMock.Object,
                CommandConverterMock.Object,
                default);
        }

        public override void SetupParameters()
        {
            _contract = new DomainTestBuilder<AddItemsToShoppingListsContract>().Create();
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterShoppingListEndpoints();
        }

        public override AddItemsToShoppingListsContract GetCommandConverterInput()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            return _contract;
        }
    }
}