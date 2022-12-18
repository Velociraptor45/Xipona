using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;

namespace ShoppingList.Frontend.Redux.Shared.Ports.Requests.Stores
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