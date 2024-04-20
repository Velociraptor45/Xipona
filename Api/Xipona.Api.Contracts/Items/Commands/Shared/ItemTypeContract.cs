﻿using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared
{
    public class ItemTypeContract
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ItemAvailabilityContract> Availabilities { get; set; }
    }
}