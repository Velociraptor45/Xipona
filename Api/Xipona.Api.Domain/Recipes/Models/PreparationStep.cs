using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Modifications;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Models;

public class PreparationStep : IPreparationStep
{
    public PreparationStep(PreparationStepId id, PreparationStepInstruction instruction, int sortingIndex)
    {
        Id = id;
        Instruction = instruction;
        SortingIndex = sortingIndex;
    }

    public PreparationStepId Id { get; }
    public PreparationStepInstruction Instruction { get; }
    public int SortingIndex { get; }

    public IPreparationStep Modify(PreparationStepModification modification)
    {
        return new PreparationStep(Id, modification.Instruction, modification.SortingIndex);
    }
}