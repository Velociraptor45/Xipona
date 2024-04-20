using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Items;

public class ItemAvailabilityContractConverter :
    IToContractConverter<ItemAvailabilityReadModel, ItemAvailabilityContract>
{
    private readonly IToContractConverter<ItemStoreReadModel, ItemStoreContract> _itemStoreContractConverter;
    private readonly IToContractConverter<ItemSectionReadModel, ItemSectionContract> _sectionContractConverter;

    public ItemAvailabilityContractConverter(
        IToContractConverter<ItemStoreReadModel, ItemStoreContract> itemStoreContractConverter,
        IToContractConverter<ItemSectionReadModel, ItemSectionContract> sectionContractConverter)
    {
        _itemStoreContractConverter = itemStoreContractConverter;
        _sectionContractConverter = sectionContractConverter;
    }

    public ItemAvailabilityContract ToContract(ItemAvailabilityReadModel source)
    {
        return new ItemAvailabilityContract(
            _itemStoreContractConverter.ToContract(source.Store),
            source.Price,
            _sectionContractConverter.ToContract(source.DefaultSection));
    }
}