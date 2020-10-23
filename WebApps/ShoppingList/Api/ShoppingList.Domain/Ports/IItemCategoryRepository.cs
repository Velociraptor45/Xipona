﻿using ShoppingList.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Ports
{
    public interface IItemCategoryRepository
    {
        Task<IEnumerable<ItemCategory>> FindByAsync(string searchInput, CancellationToken cancellationToken);
        Task<ItemCategory> FindByAsync(ItemCategoryId id, CancellationToken cancellationToken);
    }
}