using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models.Factories;
using System;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain
{
    public class ItemCategoryConverter : IToDomainConverter<ItemCategories.Entities.ItemCategory, IItemCategory>
    {
        private readonly IItemCategoryFactory itemCategoryFactory;

        public ItemCategoryConverter(IItemCategoryFactory itemCategoryFactory)
        {
            this.itemCategoryFactory = itemCategoryFactory;
        }

        public IItemCategory ToDomain(ItemCategories.Entities.ItemCategory source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return itemCategoryFactory.Create(
                new ItemCategoryId(source.Id),
                source.Name,
                source.Deleted);
        }
    }
}