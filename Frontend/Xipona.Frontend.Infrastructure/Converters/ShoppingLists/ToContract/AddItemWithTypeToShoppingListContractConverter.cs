using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
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