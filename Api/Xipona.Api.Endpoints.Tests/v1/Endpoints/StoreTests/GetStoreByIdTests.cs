using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ProjectHermes.Xipona.Api.ApplicationServices.Stores.Queries.StoreById;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Get;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Endpoints.StoreTests;

public class GetStoreByIdTests : EndpointQueryNoConverterTestsBase<GetStoreByIdQuery, IStore, StoreContract,
    GetStoreByIdTests.GetStoreByIdFixture>
{
    public GetStoreByIdTests() : base(new GetStoreByIdFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.StoreNotFound)]
    public async Task EndpointCall_WithDomainException_ShouldReturnNotFound(ErrorReasonCode errorCode)
    {
        // Arrange
        Fixture.SetupQuery();
        Fixture.SetupDomainException(errorCode);
        Fixture.SetupDomainExceptionInQueryDispatcher();
        Fixture.SetupExpectedErrorContract();
        Fixture.SetupErrorConversion();

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<NotFound<ErrorContract>>();
        var notFound = result as NotFound<ErrorContract>;
        notFound!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class GetStoreByIdFixture : EndpointQueryNoConverterFixtureBase
    {
        private readonly Guid _storeId = Guid.NewGuid();

        public GetStoreByIdFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(
                new NotFoundStatusResult(new List<ErrorReasonCode> { ErrorReasonCode.StoreNotFound }));
        }

        public override string RoutePattern => "/v1/stores/{id:guid}";

        public override async Task<IResult> ExecuteTestMethod()
        {
            return await StoreEndpoints.GetStoreById(
                _storeId,
                QueryDispatcherMock.Object,
                ContractConverterMock.Object,
                ErrorConverterMock.Object,
                default);
        }

        public override void SetupParameters()
        {
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterStoreEndpoints();
        }

        public override void SetupQuery()
        {
            Query = new GetStoreByIdQuery(new StoreId(_storeId));
        }
    }
}