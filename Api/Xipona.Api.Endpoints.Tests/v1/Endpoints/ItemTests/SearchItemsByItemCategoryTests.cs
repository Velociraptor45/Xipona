using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Xipona.Api.ApplicationServices.Items.Queries.SearchItemsByItemCategory;
using Xipona.Api.Contracts.Common;
using Xipona.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Services.Searches;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Endpoints.Tests.Common;
using Xipona.Api.Endpoints.Tests.Common.StatusResults;

namespace Xipona.Api.Endpoints.Tests.v1.Endpoints.ItemTests;

public class SearchItemsByItemCategoryTests : EndpointEnumerableQueryNoConverterTestsBase<
    SearchItemsByItemCategoryQuery, SearchItemByItemCategoryResult, SearchItemByItemCategoryResultContract,
    SearchItemsByItemCategoryTests.SearchItemsByItemCategoryFixture>
{
    public SearchItemsByItemCategoryTests() : base(new SearchItemsByItemCategoryFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.StoresNotFound)]
    [InlineData(ErrorReasonCode.ItemCategoryNotFound)]
    public async Task EndpointCall_WithDomainException_ShouldReturnNotFound(ErrorReasonCode errorCode)
    {
        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupQuery();
        Fixture.SetupDomainException(errorCode);
        Fixture.SetupDomainExceptionInQueryDispatcher();
        Fixture.SetupExpectedErrorContract();
        Fixture.SetupErrorConversion();

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<NotFound<ErrorContract>>();
        var unprocessableEntity = result as NotFound<ErrorContract>;
        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class SearchItemsByItemCategoryFixture : EndpointEnumerableQueryNoConverterFixtureBase
    {
        private readonly ItemCategoryId _itemCategoryId = ItemCategoryId.New;

        public SearchItemsByItemCategoryFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(
                ErrorReasonCode.ItemCategoryNotFound, ErrorReasonCode.StoresNotFound));
            PossibleResultsList.Add(new NotFoundStatusResult());
        }

        public override string RoutePattern => "/v1/items/search/by-item-category/{itemCategoryId:guid}";

        public override async Task<IResult> ExecuteTestMethod()
        {
            return await ItemEndpoints.SearchItemsByItemCategory(_itemCategoryId,
                QueryDispatcherMock.Object,
                ContractConverterMock.Object,
                ErrorConverterMock.Object,
                TestContext.Current.CancellationToken);
        }

        public override void SetupParameters()
        {
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterItemEndpoints();
        }

        public override void SetupQuery()
        {
            Query = new SearchItemsByItemCategoryQuery(_itemCategoryId);
        }
    }
}