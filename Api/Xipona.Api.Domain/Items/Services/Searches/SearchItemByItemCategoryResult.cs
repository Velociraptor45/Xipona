using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Queries;
using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.Domain.Items.Services.Searches;

public record SearchItemByItemCategoryResult(ItemId Id, ItemTypeId? ItemTypeId, string Name,
    ManufacturerName? ManufacturerName, IReadOnlyCollection<ItemAvailabilityReadModel> Availabilities);