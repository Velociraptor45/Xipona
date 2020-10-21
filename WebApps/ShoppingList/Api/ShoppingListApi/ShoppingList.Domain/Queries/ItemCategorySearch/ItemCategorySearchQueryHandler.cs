using ShoppingList.Domain.Converters;
using ShoppingList.Domain.Ports;
using ShoppingList.Domain.Queries.SharedModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Queries.ItemCategorySearch
{
    public class ItemCategorySearchQueryHandler : IQueryHandler<ItemCategorySearchQuery, IEnumerable<ItemCategoryReadModel>>
    {
        private readonly IShoppingListRepository shoppingListRepository;

        public ItemCategorySearchQueryHandler(IShoppingListRepository shoppingListRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
        }

        public async Task<IEnumerable<ItemCategoryReadModel>> HandleAsync(ItemCategorySearchQuery query,
            CancellationToken cancellationToken)
        {
            var itemCategoryModels = await shoppingListRepository.FindItemCategoriesByAsync(query.SearchInput,
                cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return itemCategoryModels.Select(model => model.ToReadModel());
        }
    }
}