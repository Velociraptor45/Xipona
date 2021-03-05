﻿using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain
{
    public class StoreItemAvailabilityConverterTests : ToDomainConverterTestBase<AvailableAt, IStoreItemAvailability>
    {
        protected override (AvailableAt, IStoreItemAvailability) CreateTestObjects()
        {
            var commonFixture = new CommonFixture();
            var availabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            var destination = availabilityFixture.GetAvailability();
            var source = GetSource(destination, commonFixture);

            return (source, destination);
        }

        protected override void SetupServiceCollection()
        {
            AddDependencies(serviceCollection);
        }

        public static AvailableAt GetSource(IStoreItemAvailability destination, CommonFixture commonFixture)
        {
            var store = StoreItemStoreConverterTests.GetSource(destination.Store, commonFixture);
            var defaultSection = StoreItemSectionConverterTests.GetSource(destination.DefaultSection);

            return new AvailableAt
            {
                Store = store,
                Price = destination.Price
            };
        }

        public static void AddDependencies(IServiceCollection serviceCollection)
        {
            serviceCollection.AddInstancesOfGenericType(typeof(StoreItemAvailabilityConverter).Assembly, typeof(IToDomainConverter<,>));
            serviceCollection.AddInstancesOfNonGenericType(typeof(IStoreItemAvailabilityFactory).Assembly, typeof(IStoreItemAvailabilityFactory));

            StoreItemStoreConverterTests.AddDependencies(serviceCollection);
        }
    }
}