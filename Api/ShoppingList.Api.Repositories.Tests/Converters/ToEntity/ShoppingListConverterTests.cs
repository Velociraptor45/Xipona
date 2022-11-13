using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Converters.ToEntity;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Converters.ToEntity;

public class ShoppingListConverterTests : ToEntityConverterTestBase<IShoppingList, Repositories.ShoppingLists.Entities.ShoppingList>
{
    protected override (IShoppingList, Repositories.ShoppingLists.Entities.ShoppingList) CreateTestObjects()
    {
        var source = ShoppingListMother.ThreeSections().Create();
        var destination = GetDestination(source);

        return (source, destination);
    }

    public static Repositories.ShoppingLists.Entities.ShoppingList GetDestination(IShoppingList source)
    {
        return new Repositories.ShoppingLists.Entities.ShoppingList()
        {
            Id = source.Id.Value,
            CompletionDate = source.CompletionDate,
            StoreId = source.StoreId.Value,
            ItemsOnList = source.Sections.SelectMany(section =>
                section.Items.Select(item =>
                    new ItemsOnList()
                    {
                        ShoppingListId = source.Id.Value,
                        ItemId = item.Id.Value,
                        ItemTypeId = item.TypeId?.Value,
                        InBasket = item.IsInBasket,
                        Quantity = item.Quantity.Value,
                        SectionId = section.Id.Value
                    })).ToList()
        };
    }

    protected override void SetupServiceCollection()
    {
        ServiceCollection.AddImplementationOfGenericType(typeof(ShoppingListConverter).Assembly, typeof(IToEntityConverter<,>));
    }
}