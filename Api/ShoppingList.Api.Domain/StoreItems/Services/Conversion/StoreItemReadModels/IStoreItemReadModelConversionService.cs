using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.StoreItemReadModels
{
    public interface IStoreItemReadModelConversionService
    {
        Task<StoreItemReadModel> ConvertAsync(IStoreItem item, CancellationToken cancellationToken);
    }
}