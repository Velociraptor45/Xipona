﻿using AutoMapper;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models.Factories;
using ProjectHermes.Xipona.Api.Repositories.Stores.Converters.ToDomain;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Stores.Entities;
using Section = ProjectHermes.Xipona.Api.Repositories.Stores.Entities.Section;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Stores.Converters.ToDomain;

public class SectionConverterTests : ToDomainConverterTestBase<Section, ISection, SectionConverter>
{
    protected override Section CreateSource()
    {
        return new SectionEntityBuilder().Create();
    }

    public override SectionConverter CreateSut()
    {
        return new SectionConverter(new SectionFactory());
    }

    protected override void AddMapping(IMappingExpression<Section, ISection> mapping)
    {
        mapping.As<Domain.Stores.Models.Section>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Section, Domain.Stores.Models.Section>()
            .ForCtorParam(nameof(ISection.Id), opt => opt.MapFrom(src => new SectionId(src.Id)))
            .ForCtorParam(nameof(ISection.Name), opt => opt.MapFrom(src => new SectionName(src.Name)))
            .ForCtorParam(nameof(ISection.SortingIndex), opt => opt.MapFrom(src => src.SortIndex))
            .ForCtorParam(nameof(ISection.IsDefaultSection), opt => opt.MapFrom(src => src.IsDefaultSection))
            .ForCtorParam(nameof(ISection.IsDeleted), opt => opt.MapFrom(src => src.IsDeleted));
    }
}