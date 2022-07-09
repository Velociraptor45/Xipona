using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Stores;

public class SectionContractConverter :
    IToContractConverter<SectionReadModel, SectionContract>,
    IToContractConverter<ISection, SectionContract>
{
    public SectionContract ToContract(SectionReadModel source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new SectionContract(
            source.Id.Value,
            source.Name.Value,
            source.SortingIndex,
            source.IsDefaultSection);
    }

    public SectionContract ToContract(ISection source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new SectionContract(
            source.Id.Value,
            source.Name.Value,
            source.SortingIndex,
            source.IsDefaultSection);
    }
}