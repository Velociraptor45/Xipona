using ShoppingList.Api.Domain.Extensions;
using ShoppingList.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Queries.AllQuantityInPacketTypes
{
    public class AllQuantityInPacketTypesQueryHandler : IQueryHandler<AllQuantityInPacketTypesQuery, IEnumerable<QuantityInPacketTypeReadModel>>
    {
        public Task<IEnumerable<QuantityInPacketTypeReadModel>> HandleAsync(AllQuantityInPacketTypesQuery query, CancellationToken cancellationToken)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var values = Enum.GetValues(typeof(QuantityTypeInPacket))
                .Cast<QuantityTypeInPacket>()
                .ToList();
            var readModels = values.Select(v => v.ToReadModel());

            return Task.FromResult(readModels);
        }
    }
}