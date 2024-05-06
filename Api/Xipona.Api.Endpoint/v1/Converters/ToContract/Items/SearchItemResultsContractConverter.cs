using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Shared;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Items;

public class SearchItemResultsContractConverter
    : IToContractConverter<SearchItemResultsReadModel, SearchItemResultsContract>
{
    private readonly IToContractConverter<SearchItemResultReadModel, SearchItemResultContract> _resultContractConverter;

    public SearchItemResultsContractConverter(
        IToContractConverter<SearchItemResultReadModel, SearchItemResultContract> resultContractConverter)
    {
        _resultContractConverter = resultContractConverter;
    }

    public SearchItemResultsContract ToContract(SearchItemResultsReadModel source)
    {
        return new SearchItemResultsContract(
            _resultContractConverter.ToContract(source.SearchResults),
            source.TotalResultCount);
    }
}