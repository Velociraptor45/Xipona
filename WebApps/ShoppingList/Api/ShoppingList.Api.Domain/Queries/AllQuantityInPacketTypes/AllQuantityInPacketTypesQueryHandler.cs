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
            var readModels = new List<QuantityInPacketTypeReadModel>();

            for (int i = 0; i < values.Count; i++)
            {
                var quantityType = values[i];
                readModels.Add(new QuantityInPacketTypeReadModel(i, quantityType.ToString(), quantityType.ToPriceLabel()));
            }
            return Task.FromResult(readModels.AsEnumerable());
        }
    }
}