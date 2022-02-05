using System.Linq;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Converters.ToEntity;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToEntity;

public class ShoppingListConverterTests : ToEntityConverterTestBase<IShoppingList, ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities.ShoppingList>
{
    protected override (IShoppingList, ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities.ShoppingList) CreateTestObjects()
    {
        var source = ShoppingListMother.ThreeSections().Create();
        var destination = GetDestination(source);

        return (source, destination);
    }

    public static ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities.ShoppingList GetDestination(IShoppingList source)
    {
        return new ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities.ShoppingList()
        {
            Id = source.Id.Value,
            CompletionDate = source.CompletionDate,
            StoreId = source.StoreId.Value,
            ItemsOnList = source.Sections.SelectMany(section =>
                section.Items.Select(item =>
                    new ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities.ItemsOnList()
                    {
                        ShoppingListId = source.Id.Value,
                        ItemId = item.Id.Value,
                        ItemTypeId = item.TypeId?.Value,
                        InBasket = item.IsInBasket,
                        Quantity = item.Quantity,
                        SectionId = section.Id.Value
                    })).ToList()
        };
    }

    protected override void SetupServiceCollection()
    {
        serviceCollection.AddImplementationOfGenericType(typeof(ShoppingListConverter).Assembly, typeof(IToEntityConverter<,>));
    }
}