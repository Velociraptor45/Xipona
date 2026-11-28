using Xipona.Api.Contracts.Items.Queries.Shared;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Items.Services.Searches;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.Items;

public class SearchItemResultContractConverter :
    IToContractConverter<SearchItemResultReadModel, SearchItemResultContract>
{
    public SearchItemResultContract ToContract(SearchItemResultReadModel source)
    {
        return new SearchItemResultContract(source.Id, source.ItemName, source.ManufacturerName?.Value);
    }
}