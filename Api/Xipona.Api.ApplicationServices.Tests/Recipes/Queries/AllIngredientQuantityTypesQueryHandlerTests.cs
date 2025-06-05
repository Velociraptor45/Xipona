using Xipona.Api.ApplicationServices.Recipes.Queries.AllIngredientQuantityTypes;
using Xipona.Api.ApplicationServices.Tests.Common;
using Xipona.Api.Domain.Recipes.Services.Queries.Quantities;
using Xipona.Api.Domain.TestKit.Common;
using Xipona.Api.Domain.TestKit.Recipes.Services.Queries.Quantities;
using Xipona.Api.TestTools.Exceptions;

namespace Xipona.Api.ApplicationServices.Tests.Recipes.Queries;

public class AllIngredientQuantityTypesQueryHandlerTests :
    QueryHandlerTestsBase<AllIngredientQuantityTypesQueryHandler, AllIngredientQuantityTypesQuery,
    IEnumerable<IngredientQuantityTypeReadModel>>
{
    public AllIngredientQuantityTypesQueryHandlerTests() : base(new AllIngredientQuantityTypesQueryHandlerFixture())
    {
    }

    private sealed class AllIngredientQuantityTypesQueryHandlerFixture : IQueryHandlerBaseFixture
    {
        private readonly QuantitiesQueryServiceMock _serviceMock = new(MockBehavior.Strict);

        public AllIngredientQuantityTypesQuery? Query { get; private set; }
        public IEnumerable<IngredientQuantityTypeReadModel>? ExpectedResult { get; private set; }

        public AllIngredientQuantityTypesQueryHandler CreateSut()
        {
            return new AllIngredientQuantityTypesQueryHandler(_serviceMock.Object);
        }

        public void Setup()
        {
            CreateQuery();
            CreateExpectedResult();
            SetupServiceReturningExpectedResult();
        }

        private void CreateQuery()
        {
            Query = new AllIngredientQuantityTypesQuery();
        }

        private void CreateExpectedResult()
        {
            ExpectedResult = new DomainTestBuilder<IngredientQuantityTypeReadModel>().CreateMany(2);
        }

        private void SetupServiceReturningExpectedResult()
        {
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

            _serviceMock.SetupGetIngredientAllQuantityTypes(ExpectedResult);
        }
    }
}