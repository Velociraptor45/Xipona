using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.PutItemInBasket
{
    public class PutItemInBasketContract
    {
        public int ShopingListId { get; set; }
        public ItemIdContract ItemId { get; set; }
    }
}