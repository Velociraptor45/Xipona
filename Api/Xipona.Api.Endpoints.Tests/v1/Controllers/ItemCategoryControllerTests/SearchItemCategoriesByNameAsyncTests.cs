using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Queries.ItemCategorySearch;
using ProjectHermes.Xipona.Api.Contracts.ItemCategories.Queries;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Queries;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Net.Http;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ItemCategoryControllerTests;

public class SearchItemCategoriesByNameAsyncTests : EndpointEnumerableQueryNoConverterTestsBase<
    ItemCategorySearchQuery, ItemCategorySearchResultReadModel, ItemCategorySearchResultContract,
    SearchItemCategoriesByNameAsyncTests.SearchItemCategoriesByNameAsyncFixture>
{
    public SearchItemCategoriesByNameAsyncTests() : base(new SearchItemCategoriesByNameAsyncFixture())
    {
    }

    public sealed class SearchItemCategoriesByNameAsyncFixture : EndpointEnumerableQueryNoConverterFixtureBase
    {
        private bool? _includeDeleted;
        private string? _searchInput;

        public SearchItemCategoriesByNameAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
            PossibleResultsList.Add(new BadRequestStatusResult());
        }

        public override string RoutePattern => "/v1/item-categories";
        public override HttpMethod HttpMethod => HttpMethod.Get;

        public override async Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_searchInput);
            TestPropertyNotSetException.ThrowIfNull(_includeDeleted);
            return await MinimalItemCategoryController.SearchItemCategoriesByName(
                _searchInput, _includeDeleted.Value,
                QueryDispatcherMock.Object,
                ContractConverterMock.Object,
                default);
        }

        public override void SetupParameters()
        {
            _searchInput = new TestBuilder<string>().Create();
            _includeDeleted = new TestBuilder<bool>().Create();
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterItemCategoryEndpoints();
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
            Query = new ItemCategorySearchQuery(_searchInput, _includeDeleted.Value);
        }

        public override void SetupExpectedBadRequestMessage()
        {
            ExpectedBadRequestMessage = "Search input mustn't be null or empty";
        }
    }
}