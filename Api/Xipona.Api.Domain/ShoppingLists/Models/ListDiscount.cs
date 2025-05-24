using ProjectHermes.Xipona.Api.Domain.Common.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
public record ListDiscount
{
    public ListDiscount(Percentage percentage)
    {
        Percentage = percentage;
    }

    public ListDiscount(decimal price)
    {
        Price = price;
    }

    public Percentage? Percentage { get; }
    public decimal? Price { get; }
}
