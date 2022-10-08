using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.TemporaryItems;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Items;

public class MakeTemporaryItemPermanentCommandConverter :
    IToDomainConverter<(Guid id, MakeTemporaryItemPermanentContract contract), MakeTemporaryItemPermanentCommand>
{
    private readonly IToDomainConverter<ItemAvailabilityContract, IItemAvailability> _itemAvailabilityConverter;

    public MakeTemporaryItemPermanentCommandConverter(
        IToDomainConverter<ItemAvailabilityContract, IItemAvailability> itemAvailabilityConverter)
    {
        _itemAvailabilityConverter = itemAvailabilityConverter;
    }

    public MakeTemporaryItemPermanentCommand ToDomain((Guid id, MakeTemporaryItemPermanentContract contract) source)
    {
        var (id, contract) = source;

        ItemQuantityInPacket? itemQuantityInPacket = null;
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
            _itemAvailabilityConverter.ToDomain(contract.Availabilities));

        return new MakeTemporaryItemPermanentCommand(permanentItem);
    }
}