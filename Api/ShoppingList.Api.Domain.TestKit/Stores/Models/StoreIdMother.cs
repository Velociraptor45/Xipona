using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Shared;

namespace ShoppingList.Api.Domain.TestKit.Stores.Models;

public static class StoreIdMother
{
    private static readonly CommonFixture _commonFixture = new CommonFixture();

    public static StoreIdBuilder OneFrom(IEnumerable<IItemAvailability> availabilities)
    {
        var id = _commonFixture.ChooseRandom(availabilities).StoreId;
        return new StoreIdBuilder().WithValue(id.Value);
    }
}