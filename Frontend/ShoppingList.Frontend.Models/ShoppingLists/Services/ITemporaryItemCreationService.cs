using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Services
{
    public interface ITemporaryItemCreationService
    {
        ShoppingListItem Create(string name, float price);
    }
}