using AutoMapper;
using ProjectHermes.Xipona.Api.Core.TestKit.Services;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models.Factories;
using ProjectHermes.Xipona.Api.Repositories.Stores.Converters.ToDomain;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Stores.Entities;
using ProjectHermes.Xipona.Api.TestTools.Extensions;
using Store = ProjectHermes.Xipona.Api.Repositories.Stores.Entities.Store;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Stores.Converters.ToDomain;

public class StoreConverterTests : ToDomainConverterTestBase<Store, IStore, StoreConverter>
{
    private readonly DateTimeServiceMock _dateTimeServiceMock = new(MockBehavior.Strict);

    protected override Store CreateSource()
    {
        return StoreEntityMother.Initial().Create();
    }

    public override StoreConverter CreateSut()
    {
        return new StoreConverter(new StoreFactory(new SectionFactory(), _dateTimeServiceMock.Object),
            new SectionConverter(new SectionFactory()));
    }

    protected override void AddMapping(IMappingExpression<Store, IStore> mapping)
    {
        mapping.As<Domain.Stores.Models.Store>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Store, Domain.Stores.Models.Store>()
            .ForCtorParam(nameof(IStore.Id).LowerFirstChar(), opt => opt.MapFrom(src => new StoreId(src.Id)))
            .ForCtorParam(nameof(IStore.Name).LowerFirstChar(), opt => opt.MapFrom(src => new StoreName(src.Name)))
            .ForCtorParam(nameof(IStore.Sections).LowerFirstChar(), opt => opt.MapFrom((src, ctx) =>
                new Sections(src.Sections.Select(s => ctx.Mapper.Map<ISection>(s)), new SectionFactory())))
            .ForCtorParam(nameof(IStore.IsDeleted).LowerFirstChar(), opt => opt.MapFrom(src => src.Deleted))
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => src.RowVersion))
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());

        new SectionConverterTests().AddMapping(cfg);
    }
}