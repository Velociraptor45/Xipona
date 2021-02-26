using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public class ModifyStoreRequest : IApiRequest
    {
        public ModifyStoreRequest(Guid requestId, int storeId, string name, IEnumerable<StoreSection> sections)
        {
            RequestId = requestId;
            StoreId = storeId;
            Name = name;
            Sections = sections.ToList().AsReadOnly();
        }

        public Guid RequestId { get; }
        public int StoreId { get; }
        public string Name { get; }
        public IReadOnlyCollection<StoreSection> Sections { get; }
    }
}