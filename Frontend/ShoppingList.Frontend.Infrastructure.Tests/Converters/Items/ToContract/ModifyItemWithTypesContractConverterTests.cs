using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Tests.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Tests.Converters.Items.ToContract;

public class ModifyItemWithTypesContractConverterTests :
    ToContractConverterBase<EditedItem, ModifyItemWithTypesContract, ModifyItemWithTypesContractConverter>
{
    protected override ModifyItemWithTypesContractConverter CreateSut()
    {
        return new(new ModifyItemTypeContractConverter(new ItemAvailabilityContractConverter()));
    }

    protected override void AddMapping(IMappingExpression<EditedItem, ModifyItemWithTypesContract> mapping)
    {
        mapping
            .ForCtorParam(nameof(ModifyItemWithTypesContract.Name), opt => opt.MapFrom(src => src.Name))
            .ForCtorParam(nameof(ModifyItemWithTypesContract.Comment), opt => opt.MapFrom(src => src.Comment))
            .ForCtorParam(nameof(ModifyItemWithTypesContract.QuantityType), opt => opt.MapFrom(src => src.QuantityType.Id))
            .ForCtorParam(nameof(ModifyItemWithTypesContract.QuantityInPacket), opt => opt.MapFrom(src => src.QuantityInPacket))
            .ForCtorParam(nameof(ModifyItemWithTypesContract.QuantityTypeInPacket), opt => opt.MapFrom(src => src.QuantityInPacketType!.Id))
            .ForCtorParam(nameof(ModifyItemWithTypesContract.ItemCategoryId), opt => opt.MapFrom(src => src.ItemCategoryId.Value))
            .ForCtorParam(nameof(ModifyItemWithTypesContract.ManufacturerId), opt => opt.MapFrom(src => src.ManufacturerId))
            .ForCtorParam(nameof(ModifyItemWithTypesContract.ItemTypes), opt => opt.MapFrom(src => src.ItemTypes));
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        new ModifyItemTypeContractConverterTests.WithIdFilled().AddMapping(cfg);
    }
}