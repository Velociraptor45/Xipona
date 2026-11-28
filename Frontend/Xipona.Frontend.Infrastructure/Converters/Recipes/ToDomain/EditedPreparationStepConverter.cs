using Xipona.Api.Contracts.Recipes.Queries.Get;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Recipes.States;
using System;

namespace Xipona.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class EditedPreparationStepConverter : IToDomainConverter<PreparationStepContract, EditedPreparationStep>
{
    public EditedPreparationStep ToDomain(PreparationStepContract source)
    {
        return new EditedPreparationStep(
            Guid.NewGuid(),
            source.Id,
            source.Instruction,
            source.SortingIndex);
    }
}