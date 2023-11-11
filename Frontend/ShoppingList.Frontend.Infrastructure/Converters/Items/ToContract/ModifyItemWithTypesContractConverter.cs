using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
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
}