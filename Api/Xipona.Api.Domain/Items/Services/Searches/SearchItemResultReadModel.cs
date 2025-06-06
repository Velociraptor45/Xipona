using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.Domain.Items.Services.Searches;

public record SearchItemResultReadModel(ItemId Id, ItemName ItemName, ManufacturerName? ManufacturerName);