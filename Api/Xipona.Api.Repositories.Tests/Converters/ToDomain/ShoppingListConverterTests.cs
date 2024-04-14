﻿using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.Services;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Converters.ToDomain;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Converters.ToDomain;

public class ShoppingListConverterTests : ToDomainConverterTestBase<Repositories.ShoppingLists.Entities.ShoppingList, IShoppingList>
{
    protected override (Repositories.ShoppingLists.Entities.ShoppingList, IShoppingList) CreateTestObjects()
    {
        var destination = ShoppingListMother.ThreeSections().Create();
        var source = GetSource(destination);

        return (source, destination);
    }

    protected override void SetupServiceCollection()
    {
        AddDependencies(ServiceCollection);
    }

    public static Repositories.ShoppingLists.Entities.ShoppingList GetSource(IShoppingList destination)
    {
        List<ItemsOnList> itemsOnListMap = new List<ItemsOnList>();
        foreach (var section in destination.Sections)
        {
            foreach (var item in section.Items)
            {
                var map = new ItemsOnList
                {
                    SectionId = section.Id,
                    ItemId = item.Id,
                    ItemTypeId = item.TypeId,
                    InBasket = item.IsInBasket,
                    Quantity = item.Quantity.Value
                };
                itemsOnListMap.Add(map);
            }
        }

        return new Repositories.ShoppingLists.Entities.ShoppingList
        {
            Id = destination.Id,
            StoreId = destination.StoreId,
            ItemsOnList = itemsOnListMap,
            CreatedAt = destination.CreatedAt,
            CompletionDate = destination.CompletionDate
        };
    }

    public static void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddImplementationOfGenericType(typeof(ShoppingListConverter).Assembly, typeof(IToDomainConverter<,>));
        serviceCollection.AddImplementationOfNonGenericType(typeof(IShoppingListFactory).Assembly, typeof(IShoppingListFactory));
        serviceCollection.AddImplementationOfNonGenericType(typeof(IShoppingListSectionFactory).Assembly, typeof(IShoppingListSectionFactory));
        serviceCollection.AddTransient<IDateTimeService, DateTimeService>();
    }
}