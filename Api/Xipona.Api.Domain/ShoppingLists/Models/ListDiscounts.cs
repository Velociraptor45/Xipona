namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

public class ListDiscounts : IEnumerable<ListDiscount>
{
    private readonly Dictionary<ListDiscountId, ListDiscount> _discounts;

    public ListDiscounts(IEnumerable<ListDiscount> discounts)
    {
        _discounts = discounts.ToDictionary(x => x.Id);
    }

    public void Add(ListDiscount discount)
    {
        _discounts.Add(discount.Id, discount);
    }

    public void Remove(ListDiscountId discountId)
    {
        _discounts.Remove(discountId);
    }

    public IReadOnlyCollection<ListDiscount> AsReadOnly()
    {
        return _discounts.Values;
    }

    public IEnumerator<ListDiscount> GetEnumerator()
    {
        return _discounts.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
