using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModification;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems;

public class ItemWithTypesModificationConverter : IToDomainConverter<ModifyItemWithTypesContract, ModifyItemWithTypesCommand>
{
    private readonly IToDomainConverter<ItemAvailabilityContract, IStoreItemAvailability> _availabilityConverter;

    public ItemWithTypesModificationConverter(
        IToDomainConverter<ItemAvailabilityContract, IStoreItemAvailability> availabilityConverter)
    {
        _availabilityConverter = availabilityConverter;
    }

    public ModifyItemWithTypesCommand ToDomain(ModifyItemWithTypesContract source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        var types = source.ItemTypes.Select(t => new ItemTypeModification(
            t.Id.HasValue ? new ItemTypeId(t.Id.Value) : null,
            t.Name,
            _availabilityConverter.ToDomain(t.Availabilities)));

        var modification = new ItemWithTypesModification(
            new ItemId(source.Id),
            source.Name,
            source.Comment,
            source.QuantityType.ToEnum<QuantityType>(),
            source.QuantityInPacket,
            source.QuantityTypeInPacket.ToEnum<QuantityTypeInPacket>(),
            new ItemCategoryId(source.ItemCategoryId),
            source.ManufacturerId.HasValue ?
                new ManufacturerId(source.ManufacturerId.Value) :
                null,
            types);

        return new ModifyItemWithTypesCommand(modification);
    }
}