using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.ShoppingLists.Models;

namespace Xipona.Api.ApplicationServices.ShoppingLists.Commands.RemoveShoppingListDiscount;

public record RemoveShoppingListDiscountCommand(ShoppingListId ShoppingListId, ListDiscountId ListDiscountId) : ICommand<bool>;