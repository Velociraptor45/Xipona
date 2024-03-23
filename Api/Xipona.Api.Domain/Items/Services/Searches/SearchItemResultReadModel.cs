using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;

public record SearchItemResultReadModel(ItemId Id, ItemName ItemName, ManufacturerName? ManufacturerName);