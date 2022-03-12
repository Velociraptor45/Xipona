using System;

namespace ProjectHermes.ShoppingList.Frontend.Models
{
    public class StoreSectionId
    {
        public StoreSectionId(Guid frontendId)
        {
            BackendId = Guid.Empty;
            FrontendId = frontendId;
        }

        public StoreSectionId(Guid backendId, Guid frontendId)
        {
            BackendId = backendId;
            FrontendId = frontendId;
        }

        public Guid BackendId { get; }
        public Guid FrontendId { get; set; }
    }
}