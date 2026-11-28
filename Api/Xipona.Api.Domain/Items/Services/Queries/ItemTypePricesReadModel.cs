using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Items.Services.Queries;

public record ItemTypePricesReadModel(ItemId ItemId, StoreId StoreId,
    IReadOnlyCollection<ItemTypePriceReadModel> Prices);