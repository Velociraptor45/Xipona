using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models.Factories;
using PreparationStep = ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.PreparationStep;

namespace ProjectHermes.Xipona.Api.Repositories.Recipes.Converters.ToDomain;

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