using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Stores.Models
{
    public class SectionId
    {
        public SectionId(Guid frontendId)
        {
            BackendId = Guid.Empty;
            FrontendId = frontendId;
        }

        public SectionId(Guid backendId, Guid frontendId)
        {
            BackendId = backendId;
            FrontendId = frontendId;
        }

        public Guid BackendId { get; }
        public Guid FrontendId { get; set; }
    }
}