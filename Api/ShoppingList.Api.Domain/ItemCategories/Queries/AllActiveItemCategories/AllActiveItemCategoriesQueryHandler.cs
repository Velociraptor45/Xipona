using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.AllActiveItemCategories;

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

        var results = await itemCategoryRepository.FindActiveByAsync(cancellationToken);

        return results.Select(r => r.ToReadModel());
    }
}