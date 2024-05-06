using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.SearchItems;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Shared;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ItemControllerTests;

public class SearchItemsAsyncTests :
    ControllerQueryTestsBase<ItemController, SearchItemQuery, SearchItemResultsReadModel, SearchItemResultsContract,
    SearchItemsAsyncTests.SearchItemsAsyncFixture>
{
    public SearchItemsAsyncTests() : base(new SearchItemsAsyncFixture())
    {
    }

    public sealed class SearchItemsAsyncFixture : ControllerQueryFixtureBase
    {
        private string? _searchString;
        private readonly int _page = 1;
        private int _pageSize = 30;

        public SearchItemsAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new BadRequestStatusResult());
        }

        public override MethodInfo Method =>
            typeof(ItemController).GetMethod(nameof(ItemController.SearchItemsAsync))!;

        public override ItemController CreateSut()
        {
            return new ItemController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ItemController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_searchString);
            return await sut.SearchItemsAsync(_searchString, _page, _pageSize);
        }

        public override void SetupParameters()
        {
            _searchString = new TestBuilder<string>().Create();
        }

        public override void SetupQuery()
        {
            TestPropertyNotSetException.ThrowIfNull(_searchString);

            Query = new SearchItemQuery(_searchString, _page, _pageSize);
        }

        public override void SetupParametersForBadRequest()
        {
            _searchString = new TestBuilder<string>().Create();
            _pageSize = 101;
        }

        public override void SetupExpectedBadRequestMessage()
        {
            ExpectedBadRequestMessage = "Page size cannot be greater than 100";
        }
    }
}