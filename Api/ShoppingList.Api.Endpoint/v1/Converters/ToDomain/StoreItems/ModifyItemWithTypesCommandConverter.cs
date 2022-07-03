using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems;

public class ModifyItemWithTypesCommandConverter :
    IToDomainConverter<(Guid id, ModifyItemWithTypesContract contract), ModifyItemWithTypesCommand>
{
    private readonly IToDomainConverter<ItemAvailabilityContract, IItemAvailability> _availabilityConverter;

    public ModifyItemWithTypesCommandConverter(
        IToDomainConverter<ItemAvailabilityContract, IItemAvailability> availabilityConverter)
    {
        _availabilityConverter = availabilityConverter;
    }

    public ModifyItemWithTypesCommand ToDomain((Guid id, ModifyItemWithTypesContract contract) source)
    {
        var (id, contract) = source;
        ArgumentNullException.ThrowIfNull(contract);

        var types = contract.ItemTypes.Select(t => new ItemTypeModification(
            t.Id.HasValue ? new ItemTypeId(t.Id.Value) : null,
            new ItemTypeName(t.Name),
            _availabilityConverter.ToDomain(t.Availabilities)));

        ItemQuantityInPacket? itemQuantityInPacket = null;
        //todo improve this check
        if (contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is not null)
        {
            itemQuantityInPacket = new ItemQuantityInPacket(
                new Quantity(contract.QuantityInPacket.Value),
                contract.QuantityTypeInPacket.Value.ToEnum<QuantityTypeInPacket>());
        }

        var modification = new ItemWithTypesModification(
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
            types);

        return new ModifyItemWithTypesCommand(modification);
    }
}