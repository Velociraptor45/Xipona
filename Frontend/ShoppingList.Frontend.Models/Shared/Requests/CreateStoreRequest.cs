using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public class CreateStoreRequest : IApiRequest
    {
        public CreateStoreRequest(Guid requestId, string name)
        {
            RequestId = requestId;
            Name = name;
        }

        public Guid RequestId { get; }
        public string Name { get; }
    }
}