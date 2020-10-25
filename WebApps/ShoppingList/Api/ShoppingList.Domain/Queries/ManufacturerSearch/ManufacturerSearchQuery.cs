﻿using ShoppingList.Domain.Queries.SharedModels;
using System.Collections.Generic;

namespace ShoppingList.Domain.Queries.ManufacturerSearch
{
    public class ManufacturerSearchQuery : IQuery<IEnumerable<ManufacturerReadModel>>
    {
        public ManufacturerSearchQuery(string searchInput)
        {
            SearchInput = searchInput;
        }

        public string SearchInput { get; }
    }
}