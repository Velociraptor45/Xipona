using AutoFixture;
using Xipona.Api.Contracts.TestKit.Items.Queries.AllQuantityTypes;

namespace Xipona.Api.Contracts.TestKit.Common;

public class ContractCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize(new QuantityTypeInPacketContractCustomization());
        fixture.Customize(new QuantityTypeContractCustomization());
    }
}