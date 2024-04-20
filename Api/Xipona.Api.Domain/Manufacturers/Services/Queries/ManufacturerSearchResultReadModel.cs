﻿using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Queries;

public class ManufacturerSearchResultReadModel
{
    public ManufacturerSearchResultReadModel(ManufacturerId id, ManufacturerName name)
    {
        Id = id;
        Name = name;
    }

    public ManufacturerId Id { get; }
    public ManufacturerName Name { get; }
}