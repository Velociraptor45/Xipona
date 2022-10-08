using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Recipes.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class PreparationStepConverter : IToDomainConverter<PreparationStepContract, PreparationStep>
{
    public PreparationStep ToDomain(PreparationStepContract source)
    {
        return new PreparationStep(
            source.Id,
            source.Instruction,
            source.SortingIndex);
    }
}