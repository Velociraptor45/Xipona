using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.ApplicationServices.Stores.Queries.GetActiveStoresOverview;
public record GetActiveStoresOverviewQuery : IQuery<IEnumerable<IStore>>;