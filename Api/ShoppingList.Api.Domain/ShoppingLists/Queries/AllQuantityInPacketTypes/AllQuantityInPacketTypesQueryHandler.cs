using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityInPacketTypes
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