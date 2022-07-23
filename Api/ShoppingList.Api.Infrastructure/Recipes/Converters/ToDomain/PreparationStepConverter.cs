using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;
using PreparationStep = ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities.PreparationStep;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Converters.ToDomain;

public class PreparationStepConverter : IToDomainConverter<PreparationStep, IPreparationStep>
{
    private readonly IPreparationStepFactory _preparationStepFactory;

    public PreparationStepConverter(IPreparationStepFactory preparationStepFactory)
    {
        _preparationStepFactory = preparationStepFactory;
    }

    public IPreparationStep ToDomain(PreparationStep source)
    {
        return _preparationStepFactory.Create(
            new PreparationStepId(source.Id),
            new PreparationStepInstruction(source.Instruction),
            source.SortingIndex);
    }
}