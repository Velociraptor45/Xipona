using ProjectHermes.Xipona.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using System.Linq;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Items.ToContract;

public class ModifyItemWithTypesContractConverter :
    IToContractConverter<EditedItem, ModifyItemWithTypesContract>
{
    private readonly IToContractConverter<EditedItemType, ModifyItemTypeContract> _itemTypeConverter;

    public ModifyItemWithTypesContractConverter(
        IToContractConverter<EditedItemType, ModifyItemTypeContract> itemTypeConverter)
    {
        _itemTypeConverter = itemTypeConverter;
    }

    public ModifyItemWithTypesContract ToContract(EditedItem item)
    {
        return new ModifyItemWithTypesContract(
            item.Name,
            item.Comment,
            item.QuantityType.Id,
            item.QuantityInPacket,
            item.QuantityInPacketType?.Id,
            item.ItemCategoryId.Value,
            item.ManufacturerId,
            item.ItemTypes.Select(_itemTypeConverter.ToContract));
    }
}