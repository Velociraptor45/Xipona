using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.ShoppingLists.Models;

namespace Xipona.Api.Domain.ShoppingLists.Services.Modifications;
public record TemporaryShoppingListItemReadModel(ItemId Id, bool IsInBasket, QuantityInBasket QuantityInBasket);