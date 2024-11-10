using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.RemoveItemDiscount;

public record RemoveItemDiscountCommand(ShoppingListId ShoppingListId, ItemId ItemId, ItemTypeId? ItemTypeId)
    : ICommand<bool>;
