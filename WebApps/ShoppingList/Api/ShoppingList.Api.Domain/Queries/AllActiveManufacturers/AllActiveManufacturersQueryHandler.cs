using ShoppingList.Api.Domain.Extensions;
using ShoppingList.Api.Domain.Ports;
using ShoppingList.Api.Domain.Queries.SharedModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Queries.AllActiveManufacturers
{
    public class AllActiveManufacturersQueryHandler
        : IQueryHandler<AllActiveManufacturersQuery, IEnumerable<ManufacturerReadModel>>
    {
        private readonly IManufacturerRepository manufacturerRepository;

        public AllActiveManufacturersQueryHandler(IManufacturerRepository manufacturerRepository)
        {
            this.manufacturerRepository = manufacturerRepository;
        }

        public async Task<IEnumerable<ManufacturerReadModel>> HandleAsync(AllActiveManufacturersQuery query,
            CancellationToken cancellationToken)
        {
            var manufacturers = await manufacturerRepository.FindByAsync(false, cancellationToken);

            return manufacturers.Select(m => m.ToReadModel());
        }
    }
}