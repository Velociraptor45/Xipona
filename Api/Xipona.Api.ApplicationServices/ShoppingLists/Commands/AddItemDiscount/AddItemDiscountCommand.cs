using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemDiscount;

public record AddItemDiscountCommand(ShoppingListId ShoppingListId, ItemDiscount Discount) : ICommand<bool>;