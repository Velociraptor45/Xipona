using AutoMapper;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Converters.ToDomain;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Recipes.Entities;
using ProjectHermes.Xipona.Api.TestTools.Extensions;
using PreparationStep = ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.PreparationStep;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Recipes.Converters.ToDomain;

public class PreparationStepConverterTests
    : ToDomainConverterTestBase<PreparationStep, IPreparationStep, PreparationStepConverter>
{
    public override PreparationStepConverter CreateSut()
    {
        return new(new PreparationStepFactory());
    }

    protected override PreparationStep CreateSource()
    {
        return new PreparationStepEntityBuilder().Create();
    }

    protected override void AddMapping(IMappingExpression<PreparationStep, IPreparationStep> mapping)
    {
        mapping.As<Domain.Recipes.Models.PreparationStep>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<PreparationStep, Domain.Recipes.Models.PreparationStep>()
            .ForCtorParam(nameof(IPreparationStep.Id).LowerFirstChar(), opt => opt.MapFrom(src => new PreparationStepId(src.Id)))
            .ForCtorParam(nameof(IPreparationStep.Instruction).LowerFirstChar(), opt => opt.MapFrom(src => new PreparationStepInstruction(src.Instruction)));
    }
}