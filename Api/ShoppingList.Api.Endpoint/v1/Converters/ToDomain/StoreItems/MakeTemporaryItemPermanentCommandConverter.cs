using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.TemporaryItems;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems;

public class MakeTemporaryItemPermanentCommandConverter :
    IToDomainConverter<(Guid id, MakeTemporaryItemPermanentContract contract), MakeTemporaryItemPermanentCommand>
{
    private readonly IToDomainConverter<ItemAvailabilityContract, IItemAvailability> _storeItemAvailabilityConverter;

    public MakeTemporaryItemPermanentCommandConverter(
        IToDomainConverter<ItemAvailabilityContract, IItemAvailability> storeItemAvailabilityConverter)
    {
        _storeItemAvailabilityConverter = storeItemAvailabilityConverter;
    }

    public MakeTemporaryItemPermanentCommand ToDomain((Guid id, MakeTemporaryItemPermanentContract contract) source)
    {
        var (id, contract) = source;
        ArgumentNullException.ThrowIfNull(contract);

        ItemQuantityInPacket? itemQuantityInPacket = null;
        //todo improve this check
        if (contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is not null)
        {
            itemQuantityInPacket = new ItemQuantityInPacket(
                new Quantity(contract.QuantityInPacket.Value),
                contract.QuantityTypeInPacket.Value.ToEnum<QuantityTypeInPacket>());
        }

        var permanentItem = new PermanentItem(
            new ItemId(id),
            new ItemName(contract.Name),
            new Comment(contract.Comment),
            new ItemQuantity(
                contract.QuantityType.ToEnum<QuantityType>(),
                itemQuantityInPacket),
            new ItemCategoryId(contract.ItemCategoryId),
            contract.ManufacturerId.HasValue ?
                new ManufacturerId(contract.ManufacturerId.Value) :
                null,
            _storeItemAvailabilityConverter.ToDomain(contract.Availabilities));

        return new MakeTemporaryItemPermanentCommand(permanentItem);
    }
}