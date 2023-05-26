using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Conversion.ShoppingListReadModels;

public interface IShoppingListReadModelConversionService
{
    Task<ShoppingListReadModel> ConvertAsync(IShoppingList shoppingList);
}