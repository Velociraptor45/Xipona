using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;

public class PreparationStepBuilder : DomainTestBuilderBase<PreparationStep>
{
    public PreparationStepBuilder WithId(PreparationStepId id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public PreparationStepBuilder WithInstruction(PreparationStepInstruction instruction)
    {
        FillConstructorWith(nameof(instruction), instruction);
        return this;
    }

    public PreparationStepBuilder WithSortingIndex(int sortingIndex)
    {
        FillConstructorWith(nameof(sortingIndex), sortingIndex);
        return this;
    }
}