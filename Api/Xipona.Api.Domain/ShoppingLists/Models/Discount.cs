using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
public record struct Discount(ItemId ItemId, ItemTypeId? ItemTypeId, Price Price);