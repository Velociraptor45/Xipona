using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;
using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Net.Http;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Endpoints.ManufacturerTests;

public class CreateManufacturerTests : EndpointCommandWithReturnTypeTestsBase<string,
    CreateManufacturerCommand, IManufacturer, ManufacturerContract,
    CreateManufacturerTests.CreateManufacturerFixture>
{
    public CreateManufacturerTests() : base(new CreateManufacturerFixture())
    {
    }

    public sealed class CreateManufacturerFixture : EndpointCommandWithReturnTypeFixtureBase
    {
        private string? _name;

        public CreateManufacturerFixture()
        {
            PossibleResultsList.Add(new CreatedStatusResult());
        }

        public override string RoutePattern => "/v1/manufacturers";
        public override HttpMethod HttpMethod => HttpMethod.Post;

        public override async Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_name);
            return await ManufacturerEndpoints.CreateManufacturer(_name,
                CommandDispatcherMock.Object,
                ContractConverterMock.Object,
                CommandConverterMock.Object,
                default);
        }

        public override void SetupParameters()
        {
            _name = new DomainTestBuilder<string>().Create();
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterManufacturerEndpoints();
        }

        public override string GetCommandConverterInput()
        {
            TestPropertyNotSetException.ThrowIfNull(_name);
            return _name;
        }
    }
}