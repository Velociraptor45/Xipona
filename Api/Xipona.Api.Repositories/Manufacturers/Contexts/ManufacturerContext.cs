﻿using Microsoft.EntityFrameworkCore;
using ProjectHermes.Xipona.Api.Repositories.Manufacturers.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.Manufacturers.Contexts;

public class ManufacturerContext : DbContext
{
    public DbSet<Manufacturer> Manufacturers { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public ManufacturerContext(DbContextOptions<ManufacturerContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        : base(options)
    {
    }
}