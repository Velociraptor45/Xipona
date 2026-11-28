using Xipona.Api.ApplicationServices.Recipes.Queries.RecipeById;
using Xipona.Api.ApplicationServices.Tests.Common;
using Xipona.Api.Domain.Recipes.Services.Queries;
using Xipona.Api.Domain.TestKit.Common;
using Xipona.Api.Domain.TestKit.Recipes.Services.Queries;
using Xipona.Api.TestTools.Exceptions;

namespace Xipona.Api.ApplicationServices.Tests.Recipes.Queries;

public class RecipeByIdQueryHandlerTests : QueryHandlerTestsBase<RecipeByIdQueryHandler, RecipeByIdQuery, RecipeReadModel>
{
    public RecipeByIdQueryHandlerTests() : base(new RecipeByIdQueryHandlerFixture())
    {
    }

    private class RecipeByIdQueryHandlerFixture : IQueryHandlerBaseFixture
    {
        private readonly RecipeQueryServiceMock _serviceMock = new(MockBehavior.Strict);

        public RecipeByIdQuery? Query { get; private set; }
        public RecipeReadModel? ExpectedResult { get; private set; }

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
            ExpectedResult = new DomainTestBuilder<RecipeReadModel>().Create();
        }

        private void SetupServiceReturningExpectedResult()
        {
            TestPropertyNotSetException.ThrowIfNull(Query);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

            _serviceMock.SetupGetAsync(Query.RecipeId, ExpectedResult);
        }
    }
}