﻿using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.Items;

public class ModifyItemWithTypesCommandConverter :
    IToDomainConverter<(Guid id, ModifyItemWithTypesContract contract), ModifyItemWithTypesCommand>
{
    private readonly IToDomainConverter<ItemAvailabilityContract, ItemAvailability> _availabilityConverter;

    public ModifyItemWithTypesCommandConverter(
        IToDomainConverter<ItemAvailabilityContract, ItemAvailability> availabilityConverter)
    {
        _availabilityConverter = availabilityConverter;
    }

    public ModifyItemWithTypesCommand ToDomain((Guid id, ModifyItemWithTypesContract contract) source)
    {
        var (id, contract) = source;

        var types = contract.ItemTypes.Select(t => new ItemTypeModification(
            t.Id.HasValue ? new ItemTypeId(t.Id.Value) : null,
            new ItemTypeName(t.Name),
            _availabilityConverter.ToDomain(t.Availabilities)));

        ItemQuantityInPacket? itemQuantityInPacket = null;
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