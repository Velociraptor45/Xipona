using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Shared;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Services.Queries;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Stores;

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