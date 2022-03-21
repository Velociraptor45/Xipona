using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
{
    public class AddItemWithTypeToShoppingListContractConverter :
        IToContractConverter<AddItemWithTypeToShoppingListRequest, AddItemWithTypeToShoppingListContract>
    {
        public AddItemWithTypeToShoppingListContract ToContract(AddItemWithTypeToShoppingListRequest source)
        {
            return new AddItemWithTypeToShoppingListContract
            {
                ShoppingListId = source.ShoppingListId,
                ItemId = source.ItemId,
                ItemTypeId = source.ItemTypeId,
                SectionId = source.SectionId,
                Quantity = source.Quantity
            };
        }
    }
}