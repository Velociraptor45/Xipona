﻿using ShoppingList.Api.Domain.Models;
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
            var readModels = new List<QuantityTypeReadModel>();

            for (int i = 0; i < values.Count; i++)
            {
                readModels.Add(new QuantityTypeReadModel(i, values[i].ToString()));
            }

            return Task.FromResult(readModels.AsEnumerable());
        }
    }
}