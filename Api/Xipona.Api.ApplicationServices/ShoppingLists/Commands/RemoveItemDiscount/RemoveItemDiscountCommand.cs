using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.ShoppingLists.Models;

namespace Xipona.Api.ApplicationServices.ShoppingLists.Commands.RemoveItemDiscount;

public record RemoveItemDiscountCommand(ShoppingListId ShoppingListId, ItemId ItemId, ItemTypeId? ItemTypeId)
    : ICommand<bool>;
