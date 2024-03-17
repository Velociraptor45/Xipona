using ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Commands.CreateItemWithTypes
{
    public class CreateItemTypeContract
    {
        public string Name { get; set; }
        public IEnumerable<ItemAvailabilityContract> Availabilities { get; set; }
    }
}