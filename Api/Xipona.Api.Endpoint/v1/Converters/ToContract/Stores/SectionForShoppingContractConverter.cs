using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForShopping;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Stores;

public class SectionForShoppingContractConverter : IToContractConverter<ISection, SectionForShoppingContract>
{
    public SectionForShoppingContract ToContract(ISection source)
    {
        return new SectionForShoppingContract(
            source.Id,
            source.Name,
            source.IsDefaultSection,
            source.SortingIndex);
    }
}