using ShoppingList.Frontend.Redux.Items.States;

namespace ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items
{
    public class CreateItemRequest : IApiRequest
    {
        public CreateItemRequest(Guid requestId, EditedItem item)
        {
            RequestId = requestId;
            Item = item;
        }

        public Guid RequestId { get; }
        public EditedItem Item { get; }
    }
}