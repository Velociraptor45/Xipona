namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

public class ListDiscounts : IEnumerable<ListDiscount>
{
    private readonly List<ListDiscount> _discounts;

    public ListDiscounts(IEnumerable<ListDiscount> discounts)
    {
        _discounts = discounts.ToList();
    }

    public void Add(ListDiscount discount)
    {
        _discounts.Add(discount);
    }

    public void Remove(ListDiscount discount)
    {
        _discounts.Remove(discount);
    }

    public IReadOnlyCollection<ListDiscount> AsReadOnly()
    {
        return _discounts.AsReadOnly();
    }

    public IEnumerator<ListDiscount> GetEnumerator()
    {
        return _discounts.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
