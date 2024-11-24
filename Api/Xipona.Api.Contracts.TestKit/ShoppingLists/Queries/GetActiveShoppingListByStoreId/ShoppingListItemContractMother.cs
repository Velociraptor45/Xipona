using ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.Xipona.Api.Contracts.TestKit.Common;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.ShoppingLists.Queries.GetActiveShoppingListByStoreId;

public static class ShoppingListItemContractMother
{
    public static ShoppingListItemContractBuilder Valid()
    {
        var builder = new ShoppingListItemContractBuilder();

        var quantityType = new ContractTestBuilder<QuantityTypeContract>().Create();
        builder.WithQuantityType(quantityType);

        if (quantityType.Id != 0) // Unit
        {
            builder.WithoutQuantityInPacket();
            builder.WithoutQuantityTypeInPacket();
        }

        return builder;
    }
}