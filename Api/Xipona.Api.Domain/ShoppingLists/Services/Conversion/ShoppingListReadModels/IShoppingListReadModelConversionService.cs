using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Queries;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Conversion.ShoppingListReadModels;

public interface IShoppingListReadModelConversionService
{
    Task<ShoppingListReadModel> ConvertAsync(IShoppingList shoppingList);
}