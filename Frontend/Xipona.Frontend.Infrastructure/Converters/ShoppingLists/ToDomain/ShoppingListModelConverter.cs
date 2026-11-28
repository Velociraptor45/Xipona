using Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.ShoppingList.States;
using Xipona.Frontend.Redux.ShoppingList.States.Comparer;
using System.Collections.Generic;
using System.Linq;

namespace Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain;

public class ShoppingListModelConverter : IToDomainConverter<ShoppingListContract, ShoppingListModel>
{
    private readonly IToDomainConverter<ShoppingListSectionContract, ShoppingListSection> _sectionConverter;

    public ShoppingListModelConverter(
        IToDomainConverter<ShoppingListSectionContract, ShoppingListSection> sectionConverter)
    {
        _sectionConverter = sectionConverter;
    }

    public ShoppingListModel ToDomain(ShoppingListContract source)
    {
        var sections = source.Sections.Select(_sectionConverter.ToDomain);
        var discounts = source.ShoppingListDiscounts
            .Select(d =>
            {
                if (d.DiscountPrice is null)
                    return new ShoppingListDiscount(d.Id, d.DiscountPercentage!.Value, ShoppingListDiscountType.Percentage);
                return new ShoppingListDiscount(d.Id, d.DiscountPrice.Value, ShoppingListDiscountType.Price);
            })
            .ToList();

        return new ShoppingListModel(
            source.Id,
            new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer()),
            discounts);
    }
}