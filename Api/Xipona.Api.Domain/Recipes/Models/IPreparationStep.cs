using Xipona.Api.Domain.Recipes.Services.Modifications;
using Xipona.Api.Domain.Shared.Models;

namespace Xipona.Api.Domain.Recipes.Models;

public interface IPreparationStep : ISortable
{
    PreparationStepId Id { get; }
    PreparationStepInstruction Instruction { get; }

    IPreparationStep Modify(PreparationStepModification modification);
}