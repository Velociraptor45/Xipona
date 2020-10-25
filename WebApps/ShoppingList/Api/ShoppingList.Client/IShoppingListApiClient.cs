using RestEase;
using ShoppingList.Contracts.Commands.CreateItem;
using ShoppingList.Contracts.Commands.UpdateItem;
using ShoppingList.Contracts.Queries;
using ShoppingList.Contracts.SharedContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingList.Client
{
    public interface IShoppingListApiClient
    {
        #region ShoppingListController

        [Post("shopping-list/{shoppingListId}/items/{itemId}/add/{quantity}")]
        Task AddItemToShoppingList([Path] int shoppingListId, [Path] int itemId, [Path] float quantity);

        [Post("shopping-list/{shoppingListId}/items/{itemId}/change-quantity/{quantity}")]
        Task ChangeItemQuantityOnShoppingList([Path] int shoppingListId, [Path] int itemId, [Path] float quantity);

        [Post("shopping-list/create/{storeId}")]
        Task CreatList([Path] int storeId);

        [Post("shopping-list/{shoppingListId}/finish")]
        Task FinishList([Path] int shoppingListId);

        [Get("shopping-list/active/{storeId}")]
        Task<ShoppingListContract> GetActiveShoppingListByStoreId([Path] int storeId);

        [Post("shopping-list/{shoppingListId}/items/{itemId}/put-in-basket")]
        Task PutItemInBasket([Path] int shoppingListId, [Path] int itemId);

        [Post("shopping-list/{shoppingListId}/items/{itemId}/remove-from-basket")]
        Task RemoveItemFromBasket([Path] int shoppingListId, [Path] int itemId);

        [Post("shopping-list/{shoppingListId}/items/{itemId}/remove")]
        Task RemoveItemFromShoppingList([Path] int shoppingListId, [Path] int itemId);

        #endregion ShoppingListController

        #region ItemController

        [Post("item/create")]
        Task CreateItem([Body] CreateItemContract createItemContract);

        [Get("item/search/{search-input}/{storeId}")]
        Task<IEnumerable<ItemSearchContract>> GetItemSearchResults([Path] string searchInput, [Path] int storeId);

        [Post("item/update")]
        Task UpdateItem([Body] UpdateItemContract updateItemContract);

        #endregion ItemController
    }
}