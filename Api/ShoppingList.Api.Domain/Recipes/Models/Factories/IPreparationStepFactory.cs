using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;

public interface IPreparationStepFactory
{
    IPreparationStep CreateNew(PreparationStepCreation creation);
}