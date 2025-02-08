using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Queries.AllActiveManufacturers;
using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Shared;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Endpoints.ManufacturerTests;

public class GetAllActiveManufacturersTests : EndpointEnumerableQueryNoConverterTestsBase<
    AllActiveManufacturersQuery, ManufacturerReadModel, ManufacturerContract,
    GetAllActiveManufacturersTests.GetAllActiveManufacturersFixture>
{
    public GetAllActiveManufacturersTests() : base(new GetAllActiveManufacturersFixture())
    {
    }

    public sealed class GetAllActiveManufacturersFixture : EndpointEnumerableQueryNoConverterFixtureBase
    {
        public GetAllActiveManufacturersFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override string RoutePattern => "/v1/manufacturers/active";

        public override async Task<IResult> ExecuteTestMethod()
        {
            return await ManufacturerEndpoints.GetAllActiveManufacturers(
                QueryDispatcherMock.Object,
                ContractConverterMock.Object,
                default);
        }

        public override void SetupParameters()
        {
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterManufacturerEndpoints();
        }

        public override void SetupQuery()
        {
            Query = new AllActiveManufacturersQuery();
        }
    }
}