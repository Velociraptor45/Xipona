using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain;

public class ShoppingListConverterTests : ToDomainConverterTestBase<ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities.ShoppingList, IShoppingList>
{
    protected override (ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities.ShoppingList, IShoppingList) CreateTestObjects()
    {
        var destination = ShoppingListMother.ThreeSections().Create();
        var source = GetSource(destination);

        return (source, destination);
    }

    protected override void SetupServiceCollection()
    {
        AddDependencies(serviceCollection);
    }

    public static ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities.ShoppingList GetSource(IShoppingList destination)
    {
        List<ItemsOnList> itemsOnListMap = new List<ItemsOnList>();
        foreach (var section in destination.Sections)
        {
            foreach (var item in section.Items)
            {
                var map = new ItemsOnList
                {
                    SectionId = section.Id.Value,
                    ItemId = item.Id.Value,
                    ItemTypeId = item.TypeId?.Value,
                    InBasket = item.IsInBasket,
                    Quantity = item.Quantity
                };
                itemsOnListMap.Add(map);
            }
        }

        return new ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities.ShoppingList
        {
            Id = destination.Id.Value,
            StoreId = destination.StoreId.Value,
            ItemsOnList = itemsOnListMap,
            CompletionDate = destination.CompletionDate
        };
    }

    public static void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddImplementationOfGenericType(typeof(ShoppingListConverter).Assembly, typeof(IToDomainConverter<,>));
        serviceCollection.AddImplementationOfNonGenericType(typeof(IShoppingListFactory).Assembly, typeof(IShoppingListFactory));
        serviceCollection.AddImplementationOfNonGenericType(typeof(IShoppingListSectionFactory).Assembly, typeof(IShoppingListSectionFactory));
        serviceCollection.AddImplementationOfNonGenericType(typeof(IShoppingListItemFactory).Assembly, typeof(IShoppingListItemFactory));
    }
}