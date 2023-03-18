using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresForShopping;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Stores;

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