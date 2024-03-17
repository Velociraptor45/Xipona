using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Stores.Queries.GetActiveStoresOverview;
public record GetActiveStoresOverviewQuery : IQuery<IEnumerable<IStore>>;