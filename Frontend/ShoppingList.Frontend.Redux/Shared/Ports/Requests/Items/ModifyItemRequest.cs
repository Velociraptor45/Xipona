using ShoppingList.Frontend.Redux.Items.States;

namespace ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items
{
    public class ModifyItemRequest : IApiRequest
    {
        public ModifyItemRequest(Guid requestId, EditedItem item)
        {
            RequestId = requestId;
            Item = item;
        }

        public Guid RequestId { get; }
        public EditedItem Item { get; }
    }
}