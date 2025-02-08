using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Queries.AllActiveItemCategories;
using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Shared;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Endpoints.ItemCategoryEndpointTests;

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
                default);
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