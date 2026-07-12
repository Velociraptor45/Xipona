using Xipona.Api.Contracts.Items.Queries.SearchItemsForShoppingLists;
using Xipona.Api.Contracts.Stores.Queries.Shared;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Items.Services.Searches;
using Xipona.Api.Domain.Stores.Services.Queries;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.Items;

public class SearchItemForShoppingListResultContractConverter :
    IToContractConverter<SearchItemForShoppingResultReadModel, SearchItemForShoppingListResultContract>
{
    private readonly IToContractConverter<SectionReadModel, SectionContract> _itemSectionContractConverter;

    public SearchItemForShoppingListResultContractConverter(
        IToContractConverter<SectionReadModel, SectionContract> itemSectionContractConverter)
    {
        _itemSectionContractConverter = itemSectionContractConverter;
    }

    public SearchItemForShoppingListResultContract ToContract(SearchItemForShoppingResultReadModel source)
    {
        return new SearchItemForShoppingListResultContract(
            source.Id,
            source.TypeId,
            source.Name,
            source.DefaultQuantity,
            source.Price,
            source.PriceLabel,
            source.ItemCategory?.Name ?? "",
            source.Manufacturer?.Name ?? "",
            _itemSectionContractConverter.ToContract(source.DefaultSection),
            source.IsFavorite);
    }
}