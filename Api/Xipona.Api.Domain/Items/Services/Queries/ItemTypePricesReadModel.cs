using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

public record ItemTypePricesReadModel(ItemId ItemId, StoreId StoreId,
    IReadOnlyCollection<ItemTypePriceReadModel> Prices);