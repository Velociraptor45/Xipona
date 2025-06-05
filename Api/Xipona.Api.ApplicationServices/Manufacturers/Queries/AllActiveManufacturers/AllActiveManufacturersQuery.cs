using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Manufacturers.Services.Shared;

namespace Xipona.Api.ApplicationServices.Manufacturers.Queries.AllActiveManufacturers;

public class AllActiveManufacturersQuery : IQuery<IEnumerable<ManufacturerReadModel>>
{
}