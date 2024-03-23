using ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared;
using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Commands.ModifyItemWithTypes
{
    public class ModifyItemTypeContract
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ItemAvailabilityContract> Availabilities { get; set; }
    }
}