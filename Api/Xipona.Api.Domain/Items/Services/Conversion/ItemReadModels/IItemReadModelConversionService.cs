using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Conversion.ItemReadModels;

public interface IItemReadModelConversionService
{
    Task<ItemReadModel> ConvertAsync(IItem item);
}