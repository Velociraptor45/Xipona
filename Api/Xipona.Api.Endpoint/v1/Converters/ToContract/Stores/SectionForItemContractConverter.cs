using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Stores;

public class SectionForItemContractConverter : IToContractConverter<ISection, SectionForItemContract>
{
    public SectionForItemContract ToContract(ISection source)
    {
        return new SectionForItemContract(
            source.Id,
            source.Name,
            source.IsDefaultSection,
            source.SortingIndex);
    }
}