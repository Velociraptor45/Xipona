using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Xipona.Api.ApplicationServices.ItemCategories.Queries.AllActiveItemCategories;
using Xipona.Api.Contracts.Common.Queries;
using Xipona.Api.Domain.ItemCategories.Services.Shared;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Endpoints.Tests.Common;
using Xipona.Api.Endpoints.Tests.Common.StatusResults;

namespace Xipona.Api.Endpoints.Tests.v1.Endpoints.ItemCategoryEndpointTests;

public class GetAllActiveItemCategoriesTests : EndpointEnumerableQueryNoConverterTestsBase<
    AllActiveItemCategoriesQuery, ItemCategoryReadModel, ItemCategoryContract,
    GetAllActiveItemCategoriesTests.GetAllActiveItemCategoriesFixture>
{
    public GetAllActiveItemCategoriesTests() : base(new GetAllActiveItemCategoriesFixture())
    {
    }

    public sealed class GetAllActiveItemCategoriesFixture : EndpointEnumerableQueryNoConverterFixtureBase
    {
        public GetAllActiveItemCategoriesFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override string RoutePattern => "/v1/item-categories/active";

        public override async Task<IResult> ExecuteTestMethod()
        {
            return await ItemCategoryEndpoints.GetAllActiveItemCategories(
                QueryDispatcherMock.Object,
                ContractConverterMock.Object,
                TestContext.Current.CancellationToken);
        }

        public override void SetupParameters()
        {
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterItemCategoryEndpoints();
        }

        public override void SetupQuery()
        {
            Query = new AllActiveItemCategoriesQuery();
        }
    }
}