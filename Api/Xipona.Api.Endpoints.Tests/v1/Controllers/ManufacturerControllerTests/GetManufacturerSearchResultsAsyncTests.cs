using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Queries.ManufacturerSearch;
using ProjectHermes.Xipona.Api.Contracts.Manufacturers.Queries;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Queries;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ManufacturerControllerTests;

public class GetManufacturerSearchResultsAsyncTests : ControllerEnumerableQueryTestsBase<ManufacturerController,
    ManufacturerSearchQuery, ManufacturerSearchResultReadModel, ManufacturerSearchResultContract,
    GetManufacturerSearchResultsAsyncTests.GetManufacturerSearchResultsAsyncFixture>
{
    public GetManufacturerSearchResultsAsyncTests() : base(new GetManufacturerSearchResultsAsyncFixture())
    {
    }

    public sealed class GetManufacturerSearchResultsAsyncFixture : ControllerEnumerableQueryFixtureBase
    {
        private bool? _includeDeleted;
        private string? _searchInput;

        public GetManufacturerSearchResultsAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
            PossibleResultsList.Add(new BadRequestStatusResult());
        }

        public override MethodInfo Method =>
            typeof(ManufacturerController).GetMethod(nameof(ManufacturerController.GetManufacturerSearchResultsAsync))!;

        public override ManufacturerController CreateSut()
        {
            return new ManufacturerController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ManufacturerController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_searchInput);
            TestPropertyNotSetException.ThrowIfNull(_includeDeleted);
            return await sut.GetManufacturerSearchResultsAsync(_searchInput, _includeDeleted.Value);
        }

        public override void SetupParameters()
        {
            _searchInput = new TestBuilder<string>().Create();
            _includeDeleted = new TestBuilder<bool>().Create();
        }

        public override void SetupParametersForBadRequest()
        {
            _searchInput = string.Empty;
            _includeDeleted = new TestBuilder<bool>().Create();
        }

        public override void SetupQuery()
        {
            TestPropertyNotSetException.ThrowIfNull(_searchInput);
            TestPropertyNotSetException.ThrowIfNull(_includeDeleted);
            Query = new ManufacturerSearchQuery(_searchInput, _includeDeleted.Value);
        }

        public override void SetupExpectedBadRequestMessage()
        {
            ExpectedBadRequestMessage = "Search input mustn't be null or empty";
        }
    }
}