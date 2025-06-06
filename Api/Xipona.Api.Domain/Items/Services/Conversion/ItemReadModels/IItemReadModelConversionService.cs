using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Queries;

namespace Xipona.Api.Domain.Items.Services.Conversion.ItemReadModels;

public interface IItemReadModelConversionService
{
    Task<ItemReadModel> ConvertAsync(IItem item);
}