using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.RemoveShoppingListDiscount;

public record RemoveShoppingListDiscountCommand(ShoppingListId ShoppingListId, ListDiscountId ListDiscountId) : ICommand<bool>;