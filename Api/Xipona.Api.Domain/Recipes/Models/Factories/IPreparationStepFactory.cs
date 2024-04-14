using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Creations;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Models.Factories;

public interface IPreparationStepFactory
{
    IPreparationStep CreateNew(PreparationStepCreation creation);

    IPreparationStep CreateNew(PreparationStepInstruction instruction, int sortingIndex);

    IPreparationStep Create(PreparationStepId id, PreparationStepInstruction instruction, int sortingIndex);
}