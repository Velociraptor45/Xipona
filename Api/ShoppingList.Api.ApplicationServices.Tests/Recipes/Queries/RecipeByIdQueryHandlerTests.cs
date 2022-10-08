using ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Queries.RecipeById;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.Common;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Services.Queries;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.Recipes.Queries;

public class RecipeByIdQueryHandlerTests : QueryHandlerTestsBase<RecipeByIdQueryHandler, RecipeByIdQuery, IRecipe>
{
    public RecipeByIdQueryHandlerTests() : base(new RecipeByIdQueryHandlerFixture())
    {
    }

    private class RecipeByIdQueryHandlerFixture : IQueryHandlerBaseFixture
    {
        private readonly RecipeQueryServiceMock _serviceMock = new(MockBehavior.Strict);

        public RecipeByIdQuery? Query { get; private set; }
        public IRecipe? ExpectedResult { get; private set; }

        public RecipeByIdQueryHandler CreateSut()
        {
            return new RecipeByIdQueryHandler(_ => _serviceMock.Object);
        }

        public void Setup()
        {
            CreateQuery();
            CreateExpectedResult();
            SetupServiceReturningExpectedResult();
        }

        private void CreateQuery()
        {
            Query = new DomainTestBuilder<RecipeByIdQuery>().Create();
        }

        private void CreateExpectedResult()
        {
            ExpectedResult = new RecipeBuilder().Create();
        }

        private void SetupServiceReturningExpectedResult()
        {
            TestPropertyNotSetException.ThrowIfNull(Query);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

            _serviceMock.SetupGetAsync(Query.RecipeId, ExpectedResult);
        }
    }
}