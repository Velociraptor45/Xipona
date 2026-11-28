using Xipona.Api.Contracts.Stores.Queries.Shared;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.Stores.Services.Queries;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.Stores;

public class SectionContractConverter :
    IToContractConverter<SectionReadModel, SectionContract>,
    IToContractConverter<ISection, SectionContract>
{
    public SectionContract ToContract(SectionReadModel source)
    {
        return new SectionContract(
            source.Id,
            source.Name,
            source.SortingIndex,
            source.IsDefaultSection);
    }

    public SectionContract ToContract(ISection source)
    {
        return new SectionContract(
            source.Id,
            source.Name,
            source.SortingIndex,
            source.IsDefaultSection);
    }
}