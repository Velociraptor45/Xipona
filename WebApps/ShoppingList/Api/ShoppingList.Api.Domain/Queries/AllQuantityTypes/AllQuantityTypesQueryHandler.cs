using ShoppingList.Api.Domain.Extensions;
using ShoppingList.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Queries.AllQuantityTypes
{
    public class AllQuantityTypesQueryHandler : IQueryHandler<AllQuantityTypesQuery, IEnumerable<QuantityTypeReadModel>>
    {
        public Task<IEnumerable<QuantityTypeReadModel>> HandleAsync(AllQuantityTypesQuery query, CancellationToken cancellationToken)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var values = Enum.GetValues(typeof(QuantityType))
                .Cast<QuantityType>()
                .ToList();
            var readModels = values.Select(v => v.ToReadModel());

            return Task.FromResult(readModels);
        }
    }
}