using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.ApplicationServices.Stores.Queries.GetActiveStoresForShopping;
public record GetActiveStoresForShoppingQuery : IQuery<IEnumerable<IStore>>;