using ShoppingList.Domain.Converters;
using ShoppingList.Domain.Ports;
using ShoppingList.Domain.Queries.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Queries.ManufacturerSearch
{
    public class ManufacturerSearchQueryHandler
        : IQueryHandler<ManufacturerSearchQuery, IEnumerable<ManufacturerReadModel>>
    {
        private readonly IShoppingListRepository shoppingListRepository;

        public ManufacturerSearchQueryHandler(IShoppingListRepository shoppingListRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
        }

        public async Task<IEnumerable<ManufacturerReadModel>> HandleAsync(ManufacturerSearchQuery query, CancellationToken cancellationToken)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var manufacturerModels = await shoppingListRepository.FindManufacturersByAsync(query.SearchInput,
                cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return manufacturerModels.Select(model => model.ToReadModel());
        }
    }
}