using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Modifications;
public record TemporaryShoppingListItemReadModel(ItemId Id, bool IsInBasket, QuantityInBasket QuantityInBasket);