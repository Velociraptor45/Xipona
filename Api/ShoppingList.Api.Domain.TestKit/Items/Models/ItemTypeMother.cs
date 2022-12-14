namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;

public static class ItemTypeMother
{
    public static ItemTypeBuilder Initial()
    {
        return new ItemTypeBuilder()
            .WithoutPredecessorId();
    }
}