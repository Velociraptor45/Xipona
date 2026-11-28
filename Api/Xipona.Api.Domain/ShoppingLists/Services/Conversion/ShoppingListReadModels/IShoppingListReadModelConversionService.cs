using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Domain.ShoppingLists.Services.Queries;

namespace Xipona.Api.Domain.ShoppingLists.Services.Conversion.ShoppingListReadModels;

public interface IShoppingListReadModelConversionService
{
    Task<ShoppingListReadModel> ConvertAsync(IShoppingList shoppingList);
}