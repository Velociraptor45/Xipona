using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Recipes.ToDomain;
using ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Common;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Converters.Recipes.ToDomain;

public class EditedPreparationStepConverterTests
    : ToDomainConverterBase<PreparationStepContract, EditedPreparationStep, EditedPreparationStepConverter>
{
    protected override EditedPreparationStepConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<PreparationStepContract, EditedPreparationStep> mapping)
    {
        mapping
            .ForCtorParam(nameof(EditedPreparationStep.Key), opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForCtorParam(nameof(EditedPreparationStep.Id), opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(EditedPreparationStep.Name), opt => opt.MapFrom(src => src.Instruction))
            .ForCtorParam(nameof(EditedPreparationStep.SortingIndex), opt => opt.MapFrom(src => src.SortingIndex));
    }
}