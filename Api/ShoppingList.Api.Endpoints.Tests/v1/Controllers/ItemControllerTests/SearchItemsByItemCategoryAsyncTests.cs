using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Queries.SearchItemsByItemCategory;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.ItemControllerTests;

public class SearchItemsByItemCategoryAsyncTests : ControllerEnumerableQueryTestsBase<ItemController,
    SearchItemsByItemCategoryQuery, SearchItemByItemCategoryResult, SearchItemByItemCategoryResultContract,
    SearchItemsByItemCategoryAsyncTests.SearchItemsByItemCategoryAsyncFixture>
{
    public SearchItemsByItemCategoryAsyncTests() : base(new SearchItemsByItemCategoryAsyncFixture())
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
        var sut = Fixture.CreateSut();

        // Act
        var result = await Fixture.ExecuteTestMethod(sut);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var unprocessableEntity = result as NotFoundObjectResult;
        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class SearchItemsByItemCategoryAsyncFixture : ControllerEnumerableQueryFixtureBase
    {
        private readonly ItemCategoryId _itemCategoryId = ItemCategoryId.New;

        public SearchItemsByItemCategoryAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(
                ErrorReasonCode.ItemCategoryNotFound, ErrorReasonCode.StoresNotFound));
            PossibleResultsList.Add(new NotFoundStatusResult());
        }

        public override MethodInfo Method =>
            typeof(ItemController).GetMethod(nameof(ItemController.SearchItemsByItemCategoryAsync))!;

        public override ItemController CreateSut()
        {
            return new ItemController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ItemController sut)
        {
            return await sut.SearchItemsByItemCategoryAsync(_itemCategoryId);
        }

        public override void SetupParameters()
        {
        }

        public override void SetupQuery()
        {
            Query = new SearchItemsByItemCategoryQuery(_itemCategoryId);
        }
    }
}