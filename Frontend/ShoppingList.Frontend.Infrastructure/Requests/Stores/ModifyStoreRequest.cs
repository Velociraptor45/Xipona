using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public class ModifyStoreRequest : IApiRequest
    {
        public ModifyStoreRequest(Guid requestId, Guid storeId, string name, IEnumerable<Section> sections)
        {
            RequestId = requestId;
            StoreId = storeId;
            Name = name;
            Sections = sections.ToList().AsReadOnly();
        }

        public Guid RequestId { get; }
        public Guid StoreId { get; }
        public string Name { get; }
        public IReadOnlyCollection<Section> Sections { get; }
    }
}