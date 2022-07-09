using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Shared;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;

public static class StoreIdMother
{
    private static readonly CommonFixture _commonFixture = new CommonFixture();

    public static StoreIdBuilder OneFrom(IEnumerable<IItemAvailability> availabilities)
    {
        var id = _commonFixture.ChooseRandom(availabilities).StoreId;
        return new StoreIdBuilder().WithValue(id.Value);
    }
}