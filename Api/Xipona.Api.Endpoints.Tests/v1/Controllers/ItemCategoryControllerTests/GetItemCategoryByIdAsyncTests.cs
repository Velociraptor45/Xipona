//using Microsoft.AspNetCore.Mvc;
//using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Queries.ItemCategoryById;
//using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
//using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
//using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
//using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
//using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
//using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
//using System.Reflection;

//namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ItemCategoryControllerTests;

//public class GetItemCategoryByIdAsyncTests : ControllerQueryTestsBase<ItemCategoryController,
//    ItemCategoryByIdQuery, IItemCategory, ItemCategoryContract,
//    GetItemCategoryByIdAsyncTests.GetItemCategoryByIdAsyncFixture>
//{
//    public GetItemCategoryByIdAsyncTests() : base(new GetItemCategoryByIdAsyncFixture())
//    {
//    }

//    [Theory]
//    [InlineData(ErrorReasonCode.ItemCategoryNotFound)]
//    public async Task EndpointCall_WithDomainException_ShouldReturnNotFound(ErrorReasonCode errorCode)
//    {
//        // Arrange
//        Fixture.SetupQuery();
//        Fixture.SetupDomainException(errorCode);
//        Fixture.SetupDomainExceptionInQueryDispatcher();
//        Fixture.SetupExpectedErrorContract();
//        Fixture.SetupErrorConversion();
//        var sut = Fixture.CreateSut();

//        // Act
//        var result = await Fixture.ExecuteTestMethod(sut);

//        // Assert
//        result.Should().BeOfType<NotFoundObjectResult>();
//        var unprocessableEntity = result as NotFoundObjectResult;
//        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
//    }

//    public sealed class GetItemCategoryByIdAsyncFixture : ControllerQueryFixtureBase
//    {
//        public GetItemCategoryByIdAsyncFixture()
//        {
//            PossibleResultsList.Add(new OkStatusResult());
//            PossibleResultsList.Add(new NotFoundStatusResult());
//            PossibleResultsList.Add(new UnprocessableEntityStatusResult(new List<ErrorReasonCode>()
//            {
//                ErrorReasonCode.ItemCategoryNotFound
//            }));
//        }

//        private readonly Guid _itemCategoryId = Guid.NewGuid();

//        public override MethodInfo Method =>
//            typeof(ItemCategoryController).GetMethod(nameof(ItemCategoryController.GetItemCategoryByIdAsync))!;

//        public override ItemCategoryController CreateSut()
//        {
//            return new ItemCategoryController(
//                QueryDispatcherMock.Object,
//                CommandDispatcherMock.Object,
//                EndpointConvertersMock.Object);
//        }

//        public override async Task<IActionResult> ExecuteTestMethod(ItemCategoryController sut)
//        {
//            return await sut.GetItemCategoryByIdAsync(_itemCategoryId);
//        }

//        public override void SetupParameters()
//        {
//        }

//        public override void SetupQuery()
//        {
//            Query = new ItemCategoryByIdQuery(new ItemCategoryId(_itemCategoryId));
//        }
//    }
//}