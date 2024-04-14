﻿using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Conversion;

public interface IItemAvailabilityReadModelConversionService
{
    Task<IDictionary<(ItemId, ItemTypeId?), IEnumerable<ItemAvailabilityReadModel>>> ConvertAsync(
        IEnumerable<IItem> items);

    Task<IDictionary<(ItemId, ItemTypeId?), IEnumerable<ItemAvailabilityReadModel>>> ConvertAsync(
        IDictionary<(ItemId, ItemTypeId?), IEnumerable<ItemAvailability>> availabilitiesDict);
}