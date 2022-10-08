using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public interface IPreparationStep : ISortable
{
    PreparationStepId Id { get; }
    PreparationStepInstruction Instruction { get; }

    IPreparationStep Modify(PreparationStepModification modification);
}