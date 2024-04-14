using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Shared;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Shared;
using ProjectHermes.Xipona.Api.Domain.Stores.Services.Queries;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;

public class SearchItemForShoppingResultReadModel
{
    public SearchItemForShoppingResultReadModel(ItemId id, ItemTypeId? typeId, string name, int defaultQuantity,
        Price price, string priceLabel, ManufacturerReadModel? manufacturer, ItemCategoryReadModel? itemCategory,
        SectionReadModel defaultSection)
    {
        Id = id;
        TypeId = typeId;
        Name = name;
        DefaultQuantity = defaultQuantity;
        Price = price;
        PriceLabel = priceLabel;
        Manufacturer = manufacturer;
        ItemCategory = itemCategory;
        DefaultSection = defaultSection;
    }

    public ItemId Id { get; }
    public ItemTypeId? TypeId { get; }
    public string Name { get; }
    public int DefaultQuantity { get; }
    public Price Price { get; }
    public string PriceLabel { get; }
    public ManufacturerReadModel? Manufacturer { get; }
    public ItemCategoryReadModel? ItemCategory { get; }
    public SectionReadModel DefaultSection { get; }
}