using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Converters.ToEntity;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Converters.ToEntity;

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
            Id = source.Id,
            CompletionDate = source.CompletionDate,
            StoreId = source.StoreId,
            CreatedAt = source.CreatedAt,
            ItemsOnList = source.Sections.SelectMany(section =>
                section.Items.Select(item =>
                    new ItemsOnList()
                    {
                        ShoppingListId = source.Id,
                        ItemId = item.Id,
                        ItemTypeId = item.TypeId,
                        InBasket = item.IsInBasket,
                        Quantity = item.Quantity.Value,
                        SectionId = section.Id
                    })).ToList()
        };
    }

    protected override void SetupServiceCollection()
    {
        ServiceCollection.AddImplementationOfGenericType(typeof(ShoppingListConverter).Assembly, typeof(IToEntityConverter<,>));
    }
}