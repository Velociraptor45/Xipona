using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Conversion.ShoppingListReadModels
{
    public interface IShoppingListReadModelConversionService
    {
        Task<ShoppingListReadModel> ConvertAsync(IShoppingList shoppingList, CancellationToken cancellationToken);
    }
}