using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Common.Models;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Repositories.ShoppingLists.Entities;
using Discount = Xipona.Api.Repositories.ShoppingLists.Entities.Discount;

namespace Xipona.Api.Repositories.ShoppingLists.Converters.ToContract;

public class ShoppingListConverter : IToContractConverter<IShoppingList, Entities.ShoppingList>
{
    public Entities.ShoppingList ToContract(IShoppingList source)
    {
        return new Entities.ShoppingList()
        {
            Id = source.Id,
            CompletionDate = source.CompletionDate,
            StoreId = source.StoreId,
            ItemsOnList = CreateItemsOnListMap(source).ToList(),
            Discounts = CreateItemDiscounts(source).ToList(),
            ListDiscounts = CreateShoppingListDiscounts(source).ToList(),
            CreatedAt = source.CreatedAt,
            RowVersion = ((AggregateRoot)source).RowVersion
        };
    }

    private static IEnumerable<ItemsOnList> CreateItemsOnListMap(IShoppingList source)
    {
        foreach (var section in source.Sections)
        {
            foreach (var item in section.Items)
            {
                yield return new ItemsOnList
                {
                    ShoppingListId = source.Id,
                    ItemId = item.Id,
                    ItemTypeId = item.TypeId,
                    InBasket = item.IsInBasket,
                    Quantity = item.Quantity.Value,
                    SectionId = section.Id
                };
            }
        }
    }

    private static IEnumerable<Discount> CreateItemDiscounts(IShoppingList source)
    {
        foreach (var discount in source.ItemDiscounts)
        {
            yield return new Discount
            {
                ShoppingListId = source.Id,
                ItemId = discount.ItemId,
                ItemTypeId = discount.ItemTypeId,
                DiscountPrice = discount.Price
            };
        }
    }

    private static IEnumerable<ShoppingListDiscount> CreateShoppingListDiscounts(IShoppingList source)
    {
        return source.ListDiscounts.Select(d => new ShoppingListDiscount
        {
            Id = d.Id,
            ShoppingListId = source.Id,
            DiscountPercentage = d.Percentage,
            DiscountPrice = d.Price
        });
    }
}