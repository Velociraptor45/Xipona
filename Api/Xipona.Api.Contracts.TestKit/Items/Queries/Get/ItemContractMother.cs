using ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.Xipona.Api.Contracts.TestKit.Items.Queries.AllQuantityTypes;
using ProjectHermes.Xipona.Api.Core.TestKit;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.Items.Queries.Get;

public static class ItemContractMother
{
    public static ItemContractBuilder Valid()
    {
        var builder = new ItemContractBuilder();

        var quantityType = new TestBuilder<QuantityTypeContract>()
            .AddCustomization(new QuantityTypeContractCustomization())
            .Create();

        builder.WithQuantityType(quantityType);

        if (quantityType.Id == 0) // Unit
        {
            builder.WithQuantityInPacket(new TestBuilder<float>().Create());
            var inPacketType = new TestBuilder<QuantityTypeInPacketContract>()
                .AddCustomization(new QuantityTypeInPacketContractCustomization())
                .Create();
            builder.WithQuantityTypeInPacket(inPacketType);
        }
        else
        {
            builder.WithoutQuantityInPacket();
            builder.WithoutQuantityTypeInPacket();
        }

        return builder;
    }
}