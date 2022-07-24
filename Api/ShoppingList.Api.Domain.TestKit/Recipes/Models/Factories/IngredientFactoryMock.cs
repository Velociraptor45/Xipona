using Moq.Language.Flow;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models.Factories;

public class IngredientFactoryMock : Mock<IIngredientFactory>
{
    public IngredientFactoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public ISetup<IIngredientFactory, Task<IIngredient>> SetupCreateNewAsync(IngredientCreation creation)
    {
        return Setup(m => m.CreateNewAsync(creation));
    }

    public void SetupCreateNewAsync(IngredientCreation creation, IIngredient returnValue)
    {
        Setup(m => m.CreateNewAsync(creation))
            .ReturnsAsync(returnValue);
    }

    public void SetupCreate(IngredientId id, ItemCategoryId itemCategoryId, IngredientQuantityType quantityType,
        IngredientQuantity quantity, IIngredient returnValue)
    {
        Setup(m => m.Create(id, itemCategoryId, quantityType, quantity))
            .Returns(returnValue);
    }
}