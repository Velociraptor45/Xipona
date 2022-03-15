﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;

public class CreateManufacturerCommand : ICommand<bool>
{
    public CreateManufacturerCommand(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace", nameof(name));
        }

        Name = name;
    }

    public string Name { get; }
}