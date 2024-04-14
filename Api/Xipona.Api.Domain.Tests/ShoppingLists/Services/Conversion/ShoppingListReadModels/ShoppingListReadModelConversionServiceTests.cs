using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Conversion.ShoppingListReadModels;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Manufacturers.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Stores.Ports;

namespace ProjectHermes.Xipona.Api.Domain.Tests.ShoppingLists.Services.Conversion.ShoppingListReadModels;

public class ShoppingListReadModelConversionServiceTests
{
    [Theory]
    [ClassData(typeof(ConvertAsyncTestData))]
    public async Task ConvertAsync_WithValidData_ShouldConvertToReadModel(IShoppingList list, IStore store,
        IEnumerable<IItem> items, IEnumerable<IItemCategory> itemCategories,
        IEnumerable<IManufacturer> manufacturers, ShoppingListReadModel expected)
    {
        // Arrange
        var storeRepositoryMock = new StoreRepositoryMock(MockBehavior.Strict);
        var itemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
        var itemCategoryRepositoryMock = new ItemCategoryRepositoryMock(MockBehavior.Strict);
        var manufacturerRepositoryMock = new ManufacturerRepositoryMock(MockBehavior.Strict);

        var service = new ShoppingListReadModelConversionService(
            storeRepositoryMock.Object,
            itemRepositoryMock.Object,
            itemCategoryRepositoryMock.Object,
            manufacturerRepositoryMock.Object);

        storeRepositoryMock.SetupFindByAsync(store.Id, store);
        itemRepositoryMock.SetupFindByAsync(items.Select(i => i.Id), items);
        itemCategoryRepositoryMock.SetupFindByAsync(itemCategories.Select(cat => cat.Id), itemCategories);
        manufacturerRepositoryMock.SetupFindByAsync(manufacturers.Select(m => m.Id), manufacturers);

        // Act
        var result = await service.ConvertAsync(list);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(expected);
        }
    }
}