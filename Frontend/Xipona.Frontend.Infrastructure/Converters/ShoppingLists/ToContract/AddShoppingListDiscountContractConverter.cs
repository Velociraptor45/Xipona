using Xipona.Api.Contracts.ShoppingLists.Commands.AddShoppingListDiscount;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.ShoppingList.States;
using System;

namespace Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToContract;

public class AddShoppingListDiscountContractConverter
    : IToContractConverter<(decimal DiscountValue, ShoppingListDiscountType Type), AddShoppingListDiscountContract>
{
    public AddShoppingListDiscountContract ToContract((decimal DiscountValue, ShoppingListDiscountType Type) source)
    {
        var (discountValue, type) = source;

        return type switch
        {
            ShoppingListDiscountType.Price => new AddShoppingListDiscountContract(discountValue, null),
            ShoppingListDiscountType.Percentage => new AddShoppingListDiscountContract(null, discountValue),
            _ => throw new ArgumentOutOfRangeException(nameof(source), type, "Unknown discount type.")
        };
    }
}
