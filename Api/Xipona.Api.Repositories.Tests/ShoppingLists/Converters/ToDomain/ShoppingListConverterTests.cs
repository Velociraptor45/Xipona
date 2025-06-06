using AutoMapper;
using Xipona.Api.Core.TestKit.Services;
using Xipona.Api.Core.Tests.Converter;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Domain.ShoppingLists.Models.Factories;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Repositories.ShoppingLists.Converters.ToDomain;
using Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;
using Xipona.Api.TestTools.Extensions;

namespace Xipona.Api.Repositories.Tests.ShoppingLists.Converters.ToDomain;

public class ShoppingListConverterTests
    : ToDomainConverterTestBase<Repositories.ShoppingLists.Entities.ShoppingList, IShoppingList, ShoppingListConverter>
{
    private readonly DateTimeServiceMock _dateTimeServiceMock = new(MockBehavior.Strict);

    public override ShoppingListConverter CreateSut()
    {
        return new(new ShoppingListFactory(new ShoppingListSectionFactory(), _dateTimeServiceMock.Object),
            new ShoppingListSectionFactory(), new ShoppingListItemConverter(), new DiscountConverter(),
            new ListDiscountConverter());
    }

    protected override Repositories.ShoppingLists.Entities.ShoppingList CreateSource()
    {
        return new ShoppingListEntityBuilder()
            .WithListDiscounts(new ShoppingListDiscountEntityBuilder()
                .WithoutDiscountPercentage()
                .CreateMany(2)
                .ToList())
            .Create();
    }

    protected override void AddMapping(IMappingExpression<Repositories.ShoppingLists.Entities.ShoppingList, IShoppingList> mapping)
    {
        mapping.As<ShoppingList>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Repositories.ShoppingLists.Entities.ShoppingList, ShoppingList>()
            .ForCtorParam(nameof(IShoppingList.Id).LowerFirstChar(), opt => opt.MapFrom(src => new ShoppingListId(src.Id)))
            .ForCtorParam(nameof(IShoppingList.StoreId).LowerFirstChar(), opt => opt.MapFrom(src => new StoreId(src.StoreId)))
            .ForCtorParam(nameof(IShoppingList.CreatedAt).LowerFirstChar(), opt => opt.MapFrom(src => src.CreatedAt))
            .ForCtorParam(nameof(IShoppingList.CompletionDate).LowerFirstChar(), opt => opt.MapFrom(src => src.CompletionDate))
            .ForCtorParam(nameof(IShoppingList.Sections).LowerFirstChar(),
                opt => opt.MapFrom((src, ctx) => src.ItemsOnList.GroupBy(
                    map => map.SectionId,
                    map => map,
                    (sectionId, maps) => new
                    {
                        SectionId = sectionId,
                        Items = maps.Select(i => ctx.Mapper.Map<ShoppingListItem>(i))
                    })
                    .Select(g => new ShoppingListSection(
                        new SectionId(g.SectionId),
                        g.Items))))
            .ForCtorParam(nameof(IShoppingList.ItemDiscounts).LowerFirstChar(),
                opt => opt.MapFrom((src, ctx) => src.Discounts.Select(d => ctx.Mapper.Map<ItemDiscount>(d))))
            .ForCtorParam(nameof(IShoppingList.ListDiscounts).LowerFirstChar(),
                opt => opt.MapFrom((src, ctx) => src.ListDiscounts.Select(d => ctx.Mapper.Map<ListDiscount>(d))))
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => src.RowVersion))
            .ForMember(dest => dest.Items, opt => opt.Ignore())
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());

        new ShoppingListItemConverterTests().AddMapping(cfg);
        new DiscountConverterTests().AddMapping(cfg);
        new ListDiscountConverterTests.WithPrice().AddMapping(cfg);
    }
}