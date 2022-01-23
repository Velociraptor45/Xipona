using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Conversion.ShoppingListReadModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Ports;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Ports;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using ShoppingList.Api.Domain.TestKit.Stores.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Services.Conversion.ShoppingListReadModels
{
    public class ShoppingListReadModelConversionServiceTests
    {
        private readonly CommonFixture commonFixture;

        public ShoppingListReadModelConversionServiceTests()
        {
            commonFixture = new CommonFixture();
        }

        [Fact]
        public async Task ConvertAsync_WithShoppingListIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var service = fixture.Create<ShoppingListReadModelConversionService>();

            // Act
            Func<Task<ShoppingListReadModel>> func = async () => await service.ConvertAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await func.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Theory]
        [ClassData(typeof(ConvertAsyncTestData))]
        public async Task ConvertAsync_WithValidData_ShouldConvertToReadModel(IShoppingList list, IStore store,
            IEnumerable<IStoreItem> items, IEnumerable<IItemCategory> itemCategories,
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
            var result = await service.ConvertAsync(list, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(expected);
            }
        }
    }
}