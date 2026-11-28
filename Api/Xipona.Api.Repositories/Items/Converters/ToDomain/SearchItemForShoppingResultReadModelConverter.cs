using Xipona.Api.Core.Attributes;
using Xipona.Api.Core.Converter;
using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.ItemCategories.Services.Shared;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Searches;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.Manufacturers.Services.Shared;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.Stores.Services.Queries;
using Xipona.Api.Repositories.Items.Entities;

namespace Xipona.Api.Repositories.Items.Converters.ToDomain;

public class SearchItemForShoppingResultReadModelConverter()
    : IToDomainConverter<ShoppingListItemViewModel, SearchItemForShoppingResultReadModel>
{
    public SearchItemForShoppingResultReadModel ToDomain(ShoppingListItemViewModel source)
    {
        var quantityType = source.ItemQuantityType.ToEnum<QuantityType>();
        
        return new SearchItemForShoppingResultReadModel(
            new ItemId(source.ItemId),
            source.ItemTypeId is null ? null : new ItemTypeId(source.ItemTypeId.Value),
            source.ItemName,
            quantityType.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
            new Price(source.Price),
            quantityType.GetAttribute<PriceLabelAttribute>().PriceLabel,
            source.ManufacturerId is null || source.ManufacturerName is null
                ? null
                : new ManufacturerReadModel(new ManufacturerId(source.ManufacturerId.Value), new ManufacturerName(source.ManufacturerName), false),
            new ItemCategoryReadModel(new ItemCategoryId(source.ItemCategoryId), new ItemCategoryName(source.ItemCategoryName), false),
            new SectionReadModel(new SectionId(source.DefaultSectionId), new SectionName(source.SectionName), 
                source.SectionSortingIndex, source.SectionIsDefaultSection),
            source.ItemIsFavorite
        );
    }
}