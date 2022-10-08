using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;

public class IngredientMock : Mock<IIngredient>
{
    public IngredientMock(IIngredient ingredient, MockBehavior behavior) : base(behavior)
    {
        SetupId(ingredient.Id);
    }

    public void SetupId(IngredientId id)
    {
        Setup(m => m.Id).Returns(id);
    }

    public void SetupModifyAsync(IngredientModification modification, IValidator validator, IIngredient returnValue)
    {
        Setup(m => m.ModifyAsync(modification, validator)).ReturnsAsync(returnValue);
    }

    public void VerifyModifyAsync(IngredientModification modification, IValidator validator, Func<Times> times)
    {
        Verify(m => m.ModifyAsync(modification, validator), times);
    }
}