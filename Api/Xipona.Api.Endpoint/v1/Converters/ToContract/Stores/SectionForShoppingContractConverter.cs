using Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForShopping;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.Stores;

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