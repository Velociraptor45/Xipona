﻿using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Ports;

public interface IItemTypeReadRepository
{
    Task<IEnumerable<(ItemId, ItemTypeId)>> FindActiveByAsync(string name, StoreId storeId,
        IEnumerable<ItemId> excludedItemIds, IEnumerable<ItemTypeId> excludedItemTypeIds, int? limit);

    Task<IEnumerable<IItemType>> FindByAsync(IEnumerable<ItemTypeId> ids);
}