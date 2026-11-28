using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.ApplicationServices.Stores.Queries.StoreById;
public record GetStoreByIdQuery(StoreId StoreId) : IQuery<IStore>;