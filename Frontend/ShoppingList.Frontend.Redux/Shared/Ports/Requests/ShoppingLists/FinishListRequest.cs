namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists
{
    public class FinishListRequest : IApiRequest
    {
        public FinishListRequest(Guid requestId, Guid shoppingListId, DateTimeOffset? finishedAt)
        {
            RequestId = requestId;
            ShoppingListId = shoppingListId;
            FinishedAt = finishedAt;
        }

        public Guid RequestId { get; }
        public Guid ShoppingListId { get; }
        public DateTimeOffset? FinishedAt { get; }
    }
}