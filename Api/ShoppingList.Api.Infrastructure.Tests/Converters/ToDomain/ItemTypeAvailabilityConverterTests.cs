﻿using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain;

internal class ItemTypeAvailabilityConverterTests : ToDomainConverterTestBase<ItemTypeAvailableAt, IItemAvailability>
{
    protected override (ItemTypeAvailableAt, IItemAvailability) CreateTestObjects()
    {
        var destination = StoreItemAvailabilityMother.Initial().Create();
        var source = GetSource(destination);

        return (source, destination);
    }

    public static ItemTypeAvailableAt GetSource(IItemAvailability destination)
    {
        return new ItemTypeAvailableAt
        {
            StoreId = destination.StoreId.Value,
            Price = destination.Price.Value,
            DefaultSectionId = destination.DefaultSectionId.Value
        };
    }

    protected override void SetupServiceCollection()
    {
        AddDependencies(ServiceCollection);
    }

    public static void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddImplementationOfNonGenericType(typeof(IItemAvailabilityFactory).Assembly,
            typeof(IItemAvailabilityFactory));
    }
}