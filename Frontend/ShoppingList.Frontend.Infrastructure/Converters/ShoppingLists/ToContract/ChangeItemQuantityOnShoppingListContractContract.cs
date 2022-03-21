using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Shared;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
{
    public class ChangeItemQuantityOnShoppingListContractContract :
        IToContractConverter<ChangeItemQuantityOnShoppingListRequest, ChangeItemQuantityOnShoppingListContract>
    {
        private readonly IToContractConverter<ItemId, ItemIdContract> itemIdConverter;

        public ChangeItemQuantityOnShoppingListContractContract(
            IToContractConverter<ItemId, ItemIdContract> itemIdConverter)
        {
            this.itemIdConverter = itemIdConverter;
        }

        public ChangeItemQuantityOnShoppingListContract ToContract(ChangeItemQuantityOnShoppingListRequest source)
        {
            return new ChangeItemQuantityOnShoppingListContract
            {
                ShoppingListId = source.ShoppingListId,
                ItemId = itemIdConverter.ToContract(source.ItemId),
                ItemTypeId = source.ItemTypeId,
                Quantity = source.Quantity
            };
        }
    }
}