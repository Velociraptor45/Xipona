using ProjectHermes.Xipona.Api.Repositories.ItemCategories.Contexts;
using ProjectHermes.Xipona.Api.Repositories.ItemCategories.Entities;
using ProjectHermes.Xipona.Api.Repositories.Items.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Items.Entities;
using ProjectHermes.Xipona.Api.Repositories.Manufacturers.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Manufacturers.Entities;
using ProjectHermes.Xipona.Api.Repositories.Stores.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Stores.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.ItemCategories.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Manufacturers.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Stores.Entities;
using System;

namespace ProjectHermes.Xipona.Api.Endpoint.IntegrationTests.Common;

internal class ItemEntityDatabaseService
{
    private readonly DatabaseFixture _databaseFixture;

    public ItemEntityDatabaseService(DatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
    }

    public async Task SaveAsync(params Item[] items)
    {
        var scope = _databaseFixture.CreateServiceScope();
        await using var itemDbContext = _databaseFixture.GetContextInstance<ItemContext>(scope);
        await itemDbContext.AddRangeAsync(items.ToList());
        await itemDbContext.SaveChangesAsync();
    }

    public async Task CreateReferencesAsync(params Item[] items)
    {
        var manufacturers = CreateManufacturers(items);
        var itemCategories = CreateItemCategories(items);
        var stores = CreateStores(items);

        var scope = _databaseFixture.CreateServiceScope();
        await using var itemCategoryDbContext = _databaseFixture.GetContextInstance<ItemCategoryContext>(scope);
        await using var manufacturerDbContext = _databaseFixture.GetContextInstance<ManufacturerContext>(scope);
        await using var storeDbContext = _databaseFixture.GetContextInstance<StoreContext>(scope);

        await itemCategoryDbContext.AddRangeAsync(itemCategories);
        await manufacturerDbContext.AddRangeAsync(manufacturers);
        await storeDbContext.AddRangeAsync(stores);

        await itemCategoryDbContext.SaveChangesAsync();
        await manufacturerDbContext.SaveChangesAsync();
        await storeDbContext.SaveChangesAsync();
    }

    private IEnumerable<Manufacturer> CreateManufacturers(IEnumerable<Item> items)
    {
        var manufacturerIds = items.Where(i => i.ManufacturerId.HasValue).Select(i => i.ManufacturerId!.Value).Distinct();

        foreach (var manufacturerId in manufacturerIds)
        {
            yield return new ManufacturerEntityBuilder().WithId(manufacturerId).WithDeleted(false).Create();
        }
    }

    private IEnumerable<ItemCategory> CreateItemCategories(IEnumerable<Item> items)
    {
        var itemCategoryIds = items.Where(i => i.ItemCategoryId.HasValue).Select(i => i.ItemCategoryId!.Value).Distinct();

        foreach (var itemCategoryId in itemCategoryIds)
        {
            yield return new ItemCategoryEntityBuilder().WithId(itemCategoryId).WithDeleted(false).Create();
        }
    }

    private IEnumerable<Store> CreateStores(IEnumerable<Item> items)
    {
        var stores = new Dictionary<Guid, List<Guid>>();

        foreach (var item in items)
        {
            if (item.AvailableAt.Count == 0)
            {
                if (item.ItemTypes.Count == 0)
                    continue;

                foreach (var type in item.ItemTypes)
                {
                    foreach (var availability in type.AvailableAt)
                    {
                        if (!stores.ContainsKey(availability.StoreId))
                            stores.Add(availability.StoreId, new List<Guid>());

                        stores[availability.StoreId].Add(availability.DefaultSectionId);
                    }
                }
                continue;
            }

            foreach (var availability in item.AvailableAt)
            {
                if (!stores.ContainsKey(availability.StoreId))
                    stores.Add(availability.StoreId, new List<Guid>());

                stores[availability.StoreId].Add(availability.DefaultSectionId);
            }
        }

        foreach ((Guid storeId, List<Guid> sectionIds) in stores)
        {
            var sections = sectionIds.Distinct()
                .Select(id => new SectionEntityBuilder()
                    .WithId(id)
                    .Create())
                .ToList();

            yield return StoreEntityMother.Initial()
                .WithId(storeId)
                .WithSections(sections)
                .Create();
        }
    }
}