using Xipona.Api.Contracts.Items.Queries.Get;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Items.Services.Queries;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.Items;

public class ItemSectionContractConverter :
    IToContractConverter<ItemSectionReadModel, ItemSectionContract>
{
    public ItemSectionContract ToContract(ItemSectionReadModel source)
    {
        return new ItemSectionContract(source.Id, source.Name, source.SortingIndex);
    }
}