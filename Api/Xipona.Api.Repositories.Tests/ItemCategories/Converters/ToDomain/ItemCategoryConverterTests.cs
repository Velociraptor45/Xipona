using AutoMapper;
using ProjectHermes.Xipona.Api.Core.TestKit.Services;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models.Factories;
using ProjectHermes.Xipona.Api.Repositories.ItemCategories.Converters.ToDomain;
using ProjectHermes.Xipona.Api.TestTools.Extensions;
using ItemCategory = ProjectHermes.Xipona.Api.Repositories.ItemCategories.Entities.ItemCategory;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.ItemCategories.Converters.ToDomain;

public class ItemCategoryConverterTests : ToDomainConverterTestBase<ItemCategory, IItemCategory, ItemCategoryConverter>
{
    private readonly DateTimeServiceMock _dateTimeServiceMock = new(MockBehavior.Strict);

    public override ItemCategoryConverter CreateSut()
    {
        return new ItemCategoryConverter(new ItemCategoryFactory(_dateTimeServiceMock.Object));
    }

    protected override void AddMapping(IMappingExpression<ItemCategory, IItemCategory> mapping)
    {
        mapping.As<Domain.ItemCategories.Models.ItemCategory>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<ItemCategory, Domain.ItemCategories.Models.ItemCategory>()
            .ForCtorParam(nameof(IItemCategory.Id).LowerFirstChar(), opt => opt.MapFrom(src => new ItemCategoryId(src.Id)))
            .ForCtorParam(nameof(IItemCategory.Name).LowerFirstChar(), opt => opt.MapFrom(src => new ItemCategoryName(src.Name)))
            .ForCtorParam(nameof(IItemCategory.IsDeleted).LowerFirstChar(), opt => opt.MapFrom(src => src.Deleted))
            .ForCtorParam(nameof(IItemCategory.CreatedAt).LowerFirstChar(), opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => src.RowVersion))
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());
    }
}