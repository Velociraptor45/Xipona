using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Items.Services.Updates;

public interface IItemUpdateService
{
    Task UpdateAsync(ItemWithTypesUpdate update);

    Task UpdateAsync(ItemUpdate update);

    Task UpdateAsync(ItemId itemId, ItemTypeId? itemTypeId, StoreId storeId, Price price);
}