using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Searches;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Items.Services.Conversion.ItemSearchReadModels;

public interface IItemSearchReadModelConversionService
{
    Task<IEnumerable<SearchItemForShoppingResultReadModel>> ConvertAsync(IEnumerable<IItem> items, IStore store);

    Task<IEnumerable<SearchItemForShoppingResultReadModel>> ConvertAsync(
        IEnumerable<ItemWithMatchingItemTypeIds> itemTypes,
        IStore store);
}