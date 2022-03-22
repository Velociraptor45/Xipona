using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models.Factories;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Converters.ToDomain;

public class ItemCategoryConverter : IToDomainConverter<Entities.ItemCategory, IItemCategory>
{
    private readonly IItemCategoryFactory _itemCategoryFactory;

    public ItemCategoryConverter(IItemCategoryFactory itemCategoryFactory)
    {
        _itemCategoryFactory = itemCategoryFactory;
    }

    public IItemCategory ToDomain(Entities.ItemCategory source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return _itemCategoryFactory.Create(
            new ItemCategoryId(source.Id),
            new ItemCategoryName(source.Name),
            source.Deleted);
    }
}