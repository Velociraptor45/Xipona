using ProjectHermes.ShoppingList.Api.Core.Converter;
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
        return _itemCategoryFactory.Create(
            new ItemCategoryId(source.Id),
            new ItemCategoryName(source.Name),
            source.Deleted);
    }
}