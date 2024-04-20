using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;

public record SearchItemByItemCategoryResult(ItemId Id, ItemTypeId? ItemTypeId, string Name,
    ManufacturerName? ManufacturerName, IReadOnlyCollection<ItemAvailabilityReadModel> Availabilities);