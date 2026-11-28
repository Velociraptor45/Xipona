using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Services.Queries;

public record ItemTypePriceReadModel(ItemTypeId Id, Price Price, ItemTypeName Name);