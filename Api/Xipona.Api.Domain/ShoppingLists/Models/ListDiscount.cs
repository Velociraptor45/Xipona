using ProjectHermes.Xipona.Api.Domain.Common.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
public record ListDiscount
{
    public ListDiscount(ListDiscountId id, Percentage percentage)
    {
        Id = id;
        Percentage = percentage;
    }

    public ListDiscount(ListDiscountId id, decimal price)
    {
        Id = id;
        Price = price;
    }

    public ListDiscountId Id { get; }
    public Percentage? Percentage { get; }
    public decimal? Price { get; }
}
