using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Stores;

public class SectionForItemContractConverter : IToContractConverter<ISection, SectionForItemContract>
{
    public SectionForItemContract ToContract(ISection source)
    {
        return new SectionForItemContract(
            source.Id,
            source.Name,
            source.SortingIndex);
    }
}