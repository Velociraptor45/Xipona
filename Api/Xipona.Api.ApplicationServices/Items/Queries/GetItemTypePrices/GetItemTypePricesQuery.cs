using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Queries;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.ApplicationServices.Items.Queries.GetItemTypePrices;
public record GetItemTypePricesQuery(ItemId ItemId, StoreId StoreId) : IQuery<ItemTypePricesReadModel>;