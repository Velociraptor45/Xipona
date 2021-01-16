using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public class ModifyStoreRequest
    {
        public ModifyStoreRequest(Guid requestId, int storeId, string name)
        {
            RequestId = requestId;
            StoreId = storeId;
            Name = name;
        }

        public Guid RequestId { get; }
        public int StoreId { get; }
        public string Name { get; }
    }
}