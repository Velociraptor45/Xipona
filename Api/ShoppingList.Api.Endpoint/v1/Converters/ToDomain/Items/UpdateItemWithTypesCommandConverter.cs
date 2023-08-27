using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.ItemUpdateWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.UpdateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Items;

public class UpdateItemWithTypesCommandConverter
    : IToDomainConverter<(Guid id, UpdateItemWithTypesContract contract), UpdateItemWithTypesCommand>
{
    private readonly IToDomainConverter<ItemAvailabilityContract, ItemAvailability> _availabilityConverter;

    public UpdateItemWithTypesCommandConverter(
        IToDomainConverter<ItemAvailabilityContract, ItemAvailability> availabilityConverter)
    {
        _availabilityConverter = availabilityConverter;
    }

    public UpdateItemWithTypesCommand ToDomain((Guid id, UpdateItemWithTypesContract contract) source)
    {
        var (id, contract) = source;

        var itemTypeUpdates = contract.ItemTypes.Select(t => new ItemTypeUpdate(
            new ItemTypeId(t.OldId),
            new ItemTypeName(t.Name),
            _availabilityConverter.ToDomain(t.Availabilities)));

        ItemQuantityInPacket? itemQuantityInPacket = null;
        if (contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is not null)
        {
            itemQuantityInPacket = new ItemQuantityInPacket(
                new Quantity(contract.QuantityInPacket.Value),
                contract.QuantityTypeInPacket.Value.ToEnum<QuantityTypeInPacket>());
        }

        var itemUpdate = new ItemWithTypesUpdate(
            new ItemId(id),
            new ItemName(contract.Name),
            new Comment(contract.Comment),
            new ItemQuantity(
                contract.QuantityType.ToEnum<QuantityType>(),
                itemQuantityInPacket),
            new ItemCategoryId(contract.ItemCategoryId),
            contract.ManufacturerId.HasValue ? new ManufacturerId(contract.ManufacturerId.Value) : null,
            itemTypeUpdates);

        return new UpdateItemWithTypesCommand(itemUpdate);
    }
}