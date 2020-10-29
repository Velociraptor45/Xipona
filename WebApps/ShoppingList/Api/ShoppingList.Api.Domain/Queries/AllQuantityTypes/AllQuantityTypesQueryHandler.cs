using ShoppingList.Api.Domain.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Queries.AllQuantityTypes
{
    public class AllQuantityTypesQueryHandler : IQueryHandler<AllQuantityTypesQuery, QuantityTypesReadModel>
    {
        public Task<QuantityTypesReadModel> HandleAsync(AllQuantityTypesQuery query, CancellationToken cancellationToken)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var values = Enum.GetValues(typeof(QuantityType)).Cast<QuantityType>();
            return Task.FromResult(new QuantityTypesReadModel(values.Select(v => v.ToString())));
        }
    }
}