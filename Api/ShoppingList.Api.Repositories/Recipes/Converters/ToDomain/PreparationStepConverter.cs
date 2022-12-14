using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;
using PreparationStep = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.PreparationStep;

namespace ProjectHermes.ShoppingList.Api.Repositories.Recipes.Converters.ToDomain;

public class PreparationStepConverter : IToDomainConverter<Entities.PreparationStep, IPreparationStep>
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