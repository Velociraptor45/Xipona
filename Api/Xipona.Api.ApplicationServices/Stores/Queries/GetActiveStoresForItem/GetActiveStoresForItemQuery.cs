using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.ApplicationServices.Stores.Queries.GetActiveStoresForItem;
public record GetActiveStoresForItemQuery : IQuery<IEnumerable<IStore>>;