using ProjectHermes.Xipona.Api.ApplicationServices.RecipeTags.Queries.GetAll;
using ProjectHermes.Xipona.Api.ApplicationServices.Tests.Common;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.RecipeTags.Services;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Tests.RecipeTags.Queries.GetAll;

public class GetAllQueryHandlerTests : QueryHandlerTestsBase<GetAllQueryHandler, GetAllQuery,
    IEnumerable<IRecipeTag>>
{
    public GetAllQueryHandlerTests() : base(new GetAllQueryHandlerFixture())
    {
    }

    private sealed class GetAllQueryHandlerFixture : IQueryHandlerBaseFixture
    {
        private readonly RecipeTagQueryServiceMock _serviceMock = new(MockBehavior.Strict);

        public GetAllQuery? Query { get; private set; }
        public IEnumerable<IRecipeTag>? ExpectedResult { get; private set; }

        public GetAllQueryHandler CreateSut()
        {
            return new GetAllQueryHandler(_ => _serviceMock.Object);
        }

        public void Setup()
        {
            CreateQuery();
            CreateExpectedResult();
            SetupServiceReturningExpectedResult();
        }

        private void CreateQuery()
        {
            Query = new GetAllQuery();
        }

        private void CreateExpectedResult()
        {
            ExpectedResult = new RecipeTagBuilder().CreateMany(2);
        }

        private void SetupServiceReturningExpectedResult()
        {
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
            _serviceMock.SetupGetAllAsync(ExpectedResult);
        }
    }
}