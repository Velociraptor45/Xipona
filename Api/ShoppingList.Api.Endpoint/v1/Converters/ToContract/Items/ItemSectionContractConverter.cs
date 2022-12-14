using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Items;

public class ItemSectionContractConverter :
    IToContractConverter<ItemSectionReadModel, ItemSectionContract>
{
    public ItemSectionContract ToContract(ItemSectionReadModel source)
    {
        return new ItemSectionContract(source.Id, source.Name, source.SortingIndex);
    }
}