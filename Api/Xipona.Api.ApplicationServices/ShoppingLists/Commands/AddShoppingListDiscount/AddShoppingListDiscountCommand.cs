using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Common.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddShoppingListDiscount;
public class AddShoppingListDiscountCommand : ICommand<bool>
{
    public AddShoppingListDiscountCommand(decimal discountPrice, Percentage discountPercentage)
    {
        DiscountPrice = discountPrice;
        DiscountPercentage = discountPercentage;
    }

    public decimal DiscountPrice { get; }
    public Percentage DiscountPercentage { get; }
}
