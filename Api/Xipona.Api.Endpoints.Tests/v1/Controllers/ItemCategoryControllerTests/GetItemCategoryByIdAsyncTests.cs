using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Queries.ItemCategoryById;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ItemCategoryControllerTests;

public class GetItemCategoryByIdTests : EndpointQueryNoConverterTestsBase<
    ItemCategoryByIdQuery, IItemCategory, ItemCategoryContract,
    GetItemCategoryByIdTests.GetItemCategoryByIdFixture>
{
    public GetItemCategoryByIdTests() : base(new GetItemCategoryByIdFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.ItemCategoryNotFound)]
    public async Task EndpointCall_WithDomainException_ShouldReturnNotFound(ErrorReasonCode errorCode)
    {
        // Arrange
        Fixture.SetupQuery();
        Fixture.SetupQueryConverter();
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

    public sealed class GetItemCategoryByIdFixture : EndpointQueryNoConverterFixtureBase
    {
        public GetItemCategoryByIdFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(new List<ErrorReasonCode>()
            {
                ErrorReasonCode.ItemCategoryNotFound
            }));
        }

        private readonly Guid _itemCategoryId = Guid.NewGuid();

        public override string RoutePattern => "/v1/item-categories/{id:guid}";

        public override async Task<IResult> ExecuteTestMethod()
        {
            return await MinimalItemCategoryController.GetItemCategoryById(
                _itemCategoryId,
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
            app.RegisterItemCategoryEndpoints();
        }

        public override void SetupQuery()
        {
            Query = new ItemCategoryByIdQuery(new ItemCategoryId(_itemCategoryId));
        }
    }
}