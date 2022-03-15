﻿using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;

public class ManufacturerReadModel
{
    public ManufacturerReadModel(ManufacturerId id, string name, bool isDeleted)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
    }

    public ManufacturerReadModel(IManufacturer manufacturer)
    {
        ArgumentNullException.ThrowIfNull(manufacturer);

        Id = manufacturer.Id;
        Name = manufacturer.Name;
        IsDeleted = manufacturer.IsDeleted;
    }

    public ManufacturerId Id { get; }
    public string Name { get; }
    public bool IsDeleted { get; }
}