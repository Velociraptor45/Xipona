using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
{
    public class AddItemWithTypeToShoppingListContractConverter :
        IToContractConverter<AddItemWithTypeToShoppingListRequest, AddItemWithTypeToShoppingListContract>
    {
        public AddItemWithTypeToShoppingListContract ToContract(AddItemWithTypeToShoppingListRequest source)
        {
            return new AddItemWithTypeToShoppingListContract(
                source.SectionId,
                source.Quantity);
        }
    }
}