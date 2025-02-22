using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

public record ItemTypePriceReadModel(ItemTypeId Id, Price Price, ItemTypeName Name);