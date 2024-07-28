using AutoFixture;
using ProjectHermes.Xipona.Api.Contracts.TestKit.Items.Queries.AllQuantityTypes;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.Common;

public class ContractCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize(new QuantityTypeInPacketContractCustomization());
        fixture.Customize(new QuantityTypeContractCustomization());
    }
}