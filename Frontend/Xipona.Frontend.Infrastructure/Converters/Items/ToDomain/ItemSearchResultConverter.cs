using Xipona.Api.Contracts.Items.Queries.Shared;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Items.States;

namespace Xipona.Frontend.Infrastructure.Converters.Items.ToDomain;

public class ItemSearchResultConverter : IToDomainConverter<SearchItemResultContract, ItemSearchResult>
{
    public ItemSearchResult ToDomain(SearchItemResultContract contract)
    {
        return new ItemSearchResult(
            contract.ItemId,
            contract.ItemName,
            contract.ManufacturerName ?? string.Empty);
    }
}