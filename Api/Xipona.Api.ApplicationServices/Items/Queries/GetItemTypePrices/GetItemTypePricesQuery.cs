using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.GetItemTypePrices;
public record GetItemTypePricesQuery(ItemId ItemId, StoreId StoreId) : IQuery<ItemTypePricesReadModel>;