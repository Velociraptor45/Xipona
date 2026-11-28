namespace Xipona.Api.Repositories.Items.Entities;

public class ShoppingListItemViewModel
{
    public Guid ItemId { get; set; }
    public Guid? ItemTypeId { get; set; }
    public required string ItemName { get; set; }
    public int ItemQuantityType { get; set; }
    public bool ItemIsFavorite { get; set; }
    public Guid? ManufacturerId { get; set; }
    public string? ManufacturerName { get; set; }
    public Guid ItemCategoryId { get; set; }
    public required string ItemCategoryName { get; set; }
    public decimal Price { get; set; }
    public Guid DefaultSectionId { get; set; }
    public Guid StoreId { get; set; }
    public required string SectionName { get; set; }
    public int SectionSortingIndex { get; set; }
    public bool SectionIsDefaultSection { get; set; }    
}