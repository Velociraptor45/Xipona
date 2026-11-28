using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Xipona.Api.ApplicationServices.ShoppingLists.Commands.RemoveItemDiscount;
using Xipona.Api.Contracts.Common;
using Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemDiscount;
using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.TestKit.Common;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Endpoints.Tests.Common;
using Xipona.Api.Endpoints.Tests.Common.StatusResults;
using Xipona.Api.TestTools.Exceptions;
using System.Net.Http;

namespace Xipona.Api.Endpoints.Tests.v1.Endpoints.ShoppingListTests;
public class RemoveItemDiscountTests : EndpointCommandTestsBase<(Guid, RemoveItemDiscountContract),
    RemoveItemDiscountCommand, bool, RemoveItemDiscountTests.RemoveItemDiscountFixture>
{
    public RemoveItemDiscountTests() : base(new RemoveItemDiscountFixture())
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
        var notFound = result as NotFound<ErrorContract>;
        notFound!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class RemoveItemDiscountFixture : EndpointCommandFixtureBase
    {
        private readonly Guid _shoppingListId = Guid.NewGuid();
        private RemoveItemDiscountContract? _contract;

        public RemoveItemDiscountFixture()
        {
            PossibleResultsList.Add(new NoContentStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(ErrorReasonCode.ShoppingListNotFound));
        }

        public override string RoutePattern => "/v1/shopping-lists/{id:guid}/items/discounts";
        public override HttpMethod HttpMethod => HttpMethod.Delete;

        public override Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);

            return ShoppingListEndpoints.RemoveItemDiscount(
                _shoppingListId, _contract,
                CommandDispatcherMock.Object,
                ErrorConverterMock.Object,
                CommandConverterMock.Object,
                TestContext.Current.CancellationToken);
        }

        public override void SetupParameters()
        {
            _contract = new DomainTestBuilder<RemoveItemDiscountContract>().Create();
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterShoppingListEndpoints();
        }

        public override (Guid, RemoveItemDiscountContract) GetCommandConverterInput()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            return (_shoppingListId, _contract);
        }
    }
}
