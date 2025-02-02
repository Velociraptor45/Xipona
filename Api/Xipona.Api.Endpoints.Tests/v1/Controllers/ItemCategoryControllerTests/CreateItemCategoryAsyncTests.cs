//using Microsoft.AspNetCore.Mvc;
//using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;
//using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
//using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
//using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
//using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
//using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
//using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
//using ProjectHermes.Xipona.Api.TestTools.Exceptions;
//using System.Reflection;

//namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ItemCategoryControllerTests;

//public class CreateItemCategoryAsyncTests : ControllerCommandWithReturnTypeTestsBase<ShoppingListController,
//    CreateItemCategoryCommand, IItemCategory, ItemCategoryContract,
//    CreateItemCategoryAsyncTests.CreateItemCategoryAsyncFixture>
//{
//    public CreateItemCategoryAsyncTests() : base(new CreateItemCategoryAsyncFixture())
//    {
//    }

//    public sealed class CreateItemCategoryAsyncFixture : ControllerCommandWithReturnTypeFixtureBase
//    {
//        private string? _name;

//        public CreateItemCategoryAsyncFixture()
//        {
//            PossibleResultsList.Add(new CreatedStatusResult());
//        }

//        public override MethodInfo Method =>
//            typeof(MinimalItemCategoryController).GetMethod(nameof(MinimalItemCategoryController.CreateItemCategory))!;

//        public override ShoppingListController CreateSut()
//        {
//            return new ShoppingListController(
//                QueryDispatcherMock.Object,
//                CommandDispatcherMock.Object,
//                EndpointConvertersMock.Object);
//        }

//        public override async Task<IActionResult> ExecuteTestMethod(ShoppingListController sut)
//        {
//            return MinimalItemCategoryController.CreateItemCategory();

//            //TestPropertyNotSetException.ThrowIfNull(_name);
//            //return await sut.CreateItemCategoryAsync(_name);
//        }

//        public override void SetupCommand()
//        {
//            TestPropertyNotSetException.ThrowIfNull(_name);
//            Command = new CreateItemCategoryCommand(new ItemCategoryName(_name));
//        }

//        public override void SetupParameters()
//        {
//            _name = new DomainTestBuilder<string>().Create();
//        }

//        public override void SetupCommandConverter()
//        {
//            TestPropertyNotSetException.ThrowIfNull(_name);
//            TestPropertyNotSetException.ThrowIfNull(Command);

//            EndpointConvertersMock.SetupToDomain(_name, Command);
//        }
//    }
//}