using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;

public record SearchItemByItemCategoryResult(ItemId Id, ItemTypeId? ItemTypeId, string Name,
    ManufacturerName? ManufacturerName, IReadOnlyCollection<ItemAvailabilityReadModel> Availabilities);