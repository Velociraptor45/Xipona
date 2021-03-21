using System;

namespace ProjectHermes.ShoppingList.Frontend.Models
{
    public class StoreSectionId
    {
        public StoreSectionId(Guid frontendId)
        {
            BackendId = 0;
            FrontendId = frontendId;
        }

        public StoreSectionId(int backendId, Guid frontendId)
        {
            BackendId = backendId;
            FrontendId = frontendId;
        }

        public int BackendId { get; }
        public Guid FrontendId { get; set; }
    }
}