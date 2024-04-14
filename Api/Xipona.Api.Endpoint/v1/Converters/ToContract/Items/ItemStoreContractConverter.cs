using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Items;

public class ItemStoreContractConverter :
    IToContractConverter<ItemStoreReadModel, ItemStoreContract>
{
    private readonly IToContractConverter<ItemSectionReadModel, ItemSectionContract> _itemSectionContractConverter;

    public ItemStoreContractConverter(
        IToContractConverter<ItemSectionReadModel, ItemSectionContract> itemSectionContractConverter)
    {
        _itemSectionContractConverter = itemSectionContractConverter;
    }

    public ItemStoreContract ToContract(ItemStoreReadModel source)
    {
        return new ItemStoreContract(
            source.Id,
            source.Name,
            _itemSectionContractConverter.ToContract(source.Sections));
    }
}