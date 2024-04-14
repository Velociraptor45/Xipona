using System;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared
{
    public class ItemAvailabilityContract
    {
        public Guid StoreId { get; set; }
        public float Price { get; set; }
        public Guid DefaultSectionId { get; set; }
    }
}