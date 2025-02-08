using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.SearchItems;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Shared;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ItemControllerTests;

public class SearchItemsTests :
    EndpointEnumerableQueryNoConverterTestsBase<SearchItemQuery, SearchItemResultReadModel, SearchItemResultContract,
        SearchItemsTests.SearchItemsFixture>
{
    public SearchItemsTests() : base(new SearchItemsFixture())
    {
    }

    public sealed class SearchItemsFixture : EndpointEnumerableQueryNoConverterFixtureBase
    {
        private string? _searchString;
        private const int _page = 1;
        private int _pageSize = 30;

        public SearchItemsFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
            PossibleResultsList.Add(new BadRequestStatusResult());
        }

        public override string RoutePattern => "/v1/items/search";

        public override async Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_searchString);
            return await ItemEndpoints.SearchItems(_searchString, _page, _pageSize,
                QueryDispatcherMock.Object,
                ContractConverterMock.Object,
                default);
        }

        public override void SetupParameters()
        {
            _searchString = new TestBuilder<string>().Create();
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterItemEndpoints();
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