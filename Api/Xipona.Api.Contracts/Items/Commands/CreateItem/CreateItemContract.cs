﻿using ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared;
using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Commands.CreateItem
{
    public class CreateItemContract
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public int QuantityType { get; set; }
        public float? QuantityInPacket { get; set; }
        public int? QuantityTypeInPacket { get; set; }
        public Guid ItemCategoryId { get; set; }
        public Guid? ManufacturerId { get; set; }
        public IEnumerable<ItemAvailabilityContract> Availabilities { get; set; }
    }
}