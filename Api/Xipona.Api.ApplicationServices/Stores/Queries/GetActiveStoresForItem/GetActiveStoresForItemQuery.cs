using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Stores.Queries.GetActiveStoresForItem;
public record GetActiveStoresForItemQuery : IQuery<IEnumerable<IStore>>;