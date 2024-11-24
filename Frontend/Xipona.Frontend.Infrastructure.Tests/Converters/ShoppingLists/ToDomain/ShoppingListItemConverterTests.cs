using ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Items.ToDomain;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain;
using ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Common;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Converters.ShoppingLists.ToDomain;
public class ShoppingListItemConverterTests
    : ToDomainConverterBase<ShoppingListItemContract, ShoppingListItem, ShoppingListItemConverter>
{
    protected override ShoppingListItemConverter CreateSut()
    {
        return new(new QuantityTypeConverter(), new QuantityTypeInPacketConverter());
    }

    protected override void AddMapping(IMappingExpression<ShoppingListItemContract, ShoppingListItem> mapping)
    {
        mapping
            .ForCtorParam(nameof(ShoppingListItem.Id), opt => opt.MapFrom(src => new ShoppingListItemId(null, src.Id)))
            .ForCtorParam(nameof(ShoppingListItem.TypeId), opt => opt.MapFrom(src => src.TypeId))
            .ForCtorParam(nameof(ShoppingListItem.Name), opt => opt.MapFrom(src => src.Name))
            .ForCtorParam(nameof(ShoppingListItem.IsTemporary), opt => opt.MapFrom(src => src.IsTemporary))
            .ForCtorParam(nameof(ShoppingListItem.PricePerQuantity), opt => opt.MapFrom(src => src.PricePerQuantity))
            .ForCtorParam(nameof(ShoppingListItem.QuantityType), opt => opt.MapFrom((src, ctx) => ctx.Mapper.Map<QuantityType>(src.QuantityType)))
            .ForCtorParam(nameof(ShoppingListItem.QuantityInPacket), opt => opt.MapFrom(src => src.QuantityInPacket))
            .ForCtorParam(nameof(ShoppingListItem.QuantityInPacketType), opt => opt.MapFrom((src, ctx) => ctx.Mapper.Map<QuantityTypeInPacket>(src.QuantityTypeInPacket)))
            .ForCtorParam(nameof(ShoppingListItem.ItemCategory), opt => opt.MapFrom(src => src.ItemCategory.Name))
            .ForCtorParam(nameof(ShoppingListItem.Manufacturer), opt => opt.MapFrom(src => src.Manufacturer.Name))
            .ForCtorParam(nameof(ShoppingListItem.IsInBasket), opt => opt.MapFrom(src => src.IsInBasket))
            .ForCtorParam(nameof(ShoppingListItem.Quantity), opt => opt.MapFrom(src => src.Quantity))
            .ForCtorParam(nameof(ShoppingListItem.Hidden), opt => opt.MapFrom(src => src.IsInBasket))
            .ForCtorParam(nameof(ShoppingListItem.IsDiscounted), opt => opt.MapFrom(src => src.IsDiscounted));
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<QuantityTypeContract, QuantityType>();
        cfg.CreateMap<QuantityTypeInPacketContract, QuantityTypeInPacket>();
    }
}
