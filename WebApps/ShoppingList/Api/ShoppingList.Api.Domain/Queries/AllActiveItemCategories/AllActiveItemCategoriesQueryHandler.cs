using ShoppingList.Api.Domain.Converters;
using ShoppingList.Api.Domain.Ports;
using ShoppingList.Api.Domain.Queries.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Queries.AllActiveItemCategories
{
    public class AllActiveItemCategoriesQueryHandler
        : IQueryHandler<AllActiveItemCategoriesQuery, IEnumerable<ItemCategoryReadModel>>
    {
        private readonly IItemCategoryRepository itemCategoryRepository;

        public AllActiveItemCategoriesQueryHandler(IItemCategoryRepository itemCategoryRepository)
        {
            this.itemCategoryRepository = itemCategoryRepository;
        }

        public async Task<IEnumerable<ItemCategoryReadModel>> HandleAsync(AllActiveItemCategoriesQuery query, CancellationToken cancellationToken)
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var results = await itemCategoryRepository.FindByAsync(false, cancellationToken);

            return results.Select(r => r.ToReadModel());
        }
    }
}