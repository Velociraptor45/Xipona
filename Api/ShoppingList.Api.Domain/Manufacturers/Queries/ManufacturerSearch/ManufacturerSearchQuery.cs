﻿using System.Collections.Generic;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.ManufacturerSearch;

public class ManufacturerSearchQuery : IQuery<IEnumerable<ManufacturerReadModel>>
{
    public ManufacturerSearchQuery(string searchInput)
    {
        SearchInput = searchInput;
    }

    public string SearchInput { get; }
}