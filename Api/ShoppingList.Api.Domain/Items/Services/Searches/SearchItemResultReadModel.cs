using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;

public record SearchItemResultReadModel(ItemId Id, ItemName ItemName, ManufacturerName? ManufacturerName);