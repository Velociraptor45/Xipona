using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
using System;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Recipes.ToDomain;

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