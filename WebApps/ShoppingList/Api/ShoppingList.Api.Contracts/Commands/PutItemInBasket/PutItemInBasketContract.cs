using ShoppingList.Api.Contracts.Commands.Shared;

namespace ShoppingList.Api.Contracts.Commands.PutItemInBasket
{
    public class PutItemInBasketContract
    {
        public int ShopingListId { get; set; }
        public ItemIdContract ItemId { get; set; }
    }
}