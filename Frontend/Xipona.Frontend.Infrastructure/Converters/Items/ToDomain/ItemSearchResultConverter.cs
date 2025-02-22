using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Shared;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Items.ToDomain;

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