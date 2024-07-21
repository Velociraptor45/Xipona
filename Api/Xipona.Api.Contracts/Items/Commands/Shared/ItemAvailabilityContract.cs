using System;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared
{
    public class ItemAvailabilityContract
    {
        public Guid StoreId { get; set; }
        public decimal Price { get; set; }
        public Guid DefaultSectionId { get; set; }
    }
}