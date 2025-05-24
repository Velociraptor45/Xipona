using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddShoppingListDiscount;

public class AddShoppingListDiscountCommand : ICommand<bool>
{
    public AddShoppingListDiscountCommand(ShoppingListId shoppingListId, ListDiscount discount)
    {
        ShoppingListId = shoppingListId;
        Discount = discount;
    }

    public ShoppingListId ShoppingListId { get; }
    public ListDiscount Discount { get; }
}
