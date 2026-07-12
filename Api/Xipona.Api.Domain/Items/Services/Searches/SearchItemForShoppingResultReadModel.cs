using Xipona.Api.Domain.ItemCategories.Services.Shared;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Manufacturers.Services.Shared;
using Xipona.Api.Domain.Stores.Services.Queries;

namespace Xipona.Api.Domain.Items.Services.Searches;

public class SearchItemForShoppingResultReadModel
{
    public SearchItemForShoppingResultReadModel(ItemId id, ItemTypeId? typeId, string name, int defaultQuantity,
        Price price, string priceLabel, ManufacturerReadModel? manufacturer, ItemCategoryReadModel? itemCategory,
        SectionReadModel defaultSection, bool isFavorite)
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
        IsFavorite = isFavorite;
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
    public bool IsFavorite { get; }
}