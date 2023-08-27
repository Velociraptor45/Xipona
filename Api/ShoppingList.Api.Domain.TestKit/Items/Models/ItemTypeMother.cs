using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;

public static class ItemTypeMother
{
    public static ItemTypeBuilder Initial()
    {
        return new ItemTypeBuilder()
            .WithIsDeleted(false)
            .WithoutPredecessorId();
    }

    public static ItemTypeBuilder InitialAvailableAt(ItemAvailability availability)
    {
        return new ItemTypeBuilder()
            .WithAvailabilities(availability.ToMonoList())
            .WithIsDeleted(false)
            .WithoutPredecessorId();
    }
}