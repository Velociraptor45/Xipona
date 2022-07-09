using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;

public class SearchItemForShoppingResultReadModel
{
    public SearchItemForShoppingResultReadModel(ItemId id, ItemTypeId? typeId, string name, int defaultQuantity,
        Price price, ManufacturerReadModel? manufacturer, ItemCategoryReadModel? itemCategory,
        SectionReadModel defaultSection)
    {
        Id = id;
        TypeId = typeId;
        Name = name;
        DefaultQuantity = defaultQuantity;
        Price = price;
        Manufacturer = manufacturer;
        ItemCategory = itemCategory;
        DefaultSection = defaultSection;
    }

    public ItemId Id { get; }
    public ItemTypeId? TypeId { get; }
    public string Name { get; }
    public int DefaultQuantity { get; }
    public Price Price { get; }
    public ManufacturerReadModel? Manufacturer { get; }
    public ItemCategoryReadModel? ItemCategory { get; }
    public SectionReadModel DefaultSection { get; }
}