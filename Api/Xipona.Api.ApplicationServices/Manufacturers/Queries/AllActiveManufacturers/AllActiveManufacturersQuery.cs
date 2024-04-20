using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Shared;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Queries.AllActiveManufacturers;

public class AllActiveManufacturersQuery : IQuery<IEnumerable<ManufacturerReadModel>>
{
}