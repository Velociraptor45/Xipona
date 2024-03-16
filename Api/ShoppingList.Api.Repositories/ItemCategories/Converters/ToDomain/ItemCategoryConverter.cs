using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models.Factories;

namespace ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Converters.ToDomain;

public class ItemCategoryConverter : IToDomainConverter<Entities.ItemCategory, IItemCategory>
{
    private readonly IItemCategoryFactory _itemCategoryFactory;

    public ItemCategoryConverter(IItemCategoryFactory itemCategoryFactory)
    {
        _itemCategoryFactory = itemCategoryFactory;
    }

    public IItemCategory ToDomain(Entities.ItemCategory source)
    {
        var itemCategory = (AggregateRoot)_itemCategoryFactory.Create(
            new ItemCategoryId(source.Id),
            new ItemCategoryName(source.Name),
            source.Deleted,
            source.CreatedAt);

        itemCategory.EnrichWithRowVersion(source.RowVersion);
        return (itemCategory as IItemCategory)!;
    }
}