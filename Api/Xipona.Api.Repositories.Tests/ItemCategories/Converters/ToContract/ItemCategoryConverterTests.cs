using AutoMapper;
using Xipona.Api.Core.Tests.Converter;
using Xipona.Api.Domain.Common.Models;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Repositories.ItemCategories.Converters.ToContract;
using ItemCategory = Xipona.Api.Repositories.ItemCategories.Entities.ItemCategory;

namespace Xipona.Api.Repositories.Tests.ItemCategories.Converters.ToContract;

public class ItemCategoryConverterTests : ToContractConverterTestBase<IItemCategory, ItemCategory, ItemCategoryConverter>
{
    protected override ItemCategoryConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<IItemCategory, ItemCategory> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dest => dest.Deleted, opt => opt.MapFrom(src => src.IsDeleted))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => ((AggregateRoot)src).RowVersion));
    }
}