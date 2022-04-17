using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public class CreateStoreRequest : IApiRequest
    {
        public CreateStoreRequest(Guid requestId, Store store)
        {
            RequestId = requestId;
            Store = store;
        }

        public Guid RequestId { get; }
        public Store Store { get; }
    }
}