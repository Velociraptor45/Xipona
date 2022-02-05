using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.ItemCategorySearch;

public class ItemCategorySearchQueryHandler : IQueryHandler<ItemCategorySearchQuery, IEnumerable<ItemCategoryReadModel>>
{
    private readonly IItemCategoryRepository itemCategoryRepository;

    public ItemCategorySearchQueryHandler(IItemCategoryRepository itemCategoryRepository)
    {
        this.itemCategoryRepository = itemCategoryRepository;
    }

    public async Task<IEnumerable<ItemCategoryReadModel>> HandleAsync(ItemCategorySearchQuery query,
        CancellationToken cancellationToken)
    {
        var itemCategoryModels = await itemCategoryRepository.FindByAsync(query.SearchInput,
            cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return itemCategoryModels.Select(model => model.ToReadModel());
    }
}