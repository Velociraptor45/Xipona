using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Conversion.ItemSearchReadModels;

public interface IItemSearchReadModelConversionService
{
    Task<IEnumerable<SearchItemForShoppingResultReadModel>> ConvertAsync(IEnumerable<IItem> items, IStore store);

    Task<IEnumerable<SearchItemForShoppingResultReadModel>> ConvertAsync(
        IEnumerable<ItemWithMatchingItemTypeIds> itemTypes,
        IStore store);
}