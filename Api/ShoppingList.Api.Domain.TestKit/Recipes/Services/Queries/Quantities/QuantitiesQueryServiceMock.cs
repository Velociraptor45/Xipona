using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries.Quantities;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Services.Queries.Quantities;

public class QuantitiesQueryServiceMock : Mock<IQuantitiesQueryService>
{
    public QuantitiesQueryServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupGetIngredientAllQuantityTypes(IEnumerable<IngredientQuantityTypeReadModel> returnValue)
    {
        Setup(m => m.GetAllIngredientQuantityTypes()).Returns(returnValue);
    }
}