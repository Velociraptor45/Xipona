using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Domain.ShoppingLists.Services.Conversion.ShoppingListReadModels;
using Xipona.Api.Domain.ShoppingLists.Services.Queries;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.TestKit.ItemCategories.Ports;
using Xipona.Api.Domain.TestKit.Items.Ports;
using Xipona.Api.Domain.TestKit.Manufacturers.Ports;
using Xipona.Api.Domain.TestKit.Stores.Ports;

namespace Xipona.Api.Domain.Tests.ShoppingLists.Services.Conversion.ShoppingListReadModels;

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

        var itemsList = items.ToList();
        var itemCategoriesList = itemCategories.ToList();
        var manufacturersList = manufacturers.ToList();
        
        var itemIds = itemsList.Select(i => i.Id).ToList();
        var itemCategoryIds = itemCategoriesList.Select(cat => cat.Id).ToList();
        var manufacturerIds = manufacturersList.Select(m => m.Id).ToList();
        
        storeRepositoryMock.SetupFindByAsync(store.Id, store);
        itemRepositoryMock.SetupFindByAsync(itemIds, itemsList);
        itemCategoryRepositoryMock.SetupFindByAsync(itemCategoryIds, itemCategoriesList);
        manufacturerRepositoryMock.SetupFindByAsync(manufacturerIds, manufacturersList);

        // Act
        var result = await service.ConvertAsync(list);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}