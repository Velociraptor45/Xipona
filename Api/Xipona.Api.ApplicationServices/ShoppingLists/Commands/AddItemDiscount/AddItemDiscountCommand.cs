using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.ShoppingLists.Models;

namespace Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemDiscount;

public record AddItemDiscountCommand(ShoppingListId ShoppingListId, ItemDiscount Discount) : ICommand<bool>;