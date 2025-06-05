using Xipona.Api.Contracts.Items.Queries.Get;
using Xipona.Api.Core.TestKit;
using Xipona.Api.Domain.TestKit.Shared;

namespace Xipona.Api.Contracts.TestKit.Items.Queries.Get;

public static class ItemAvailabilityContractMother
{
    public static ItemAvailabilityContractBuilder Valid()
    {
        var builder = new ItemAvailabilityContractBuilder();

        var store = new TestBuilder<ItemStoreContract>().Create();

        builder.WithStore(store);

        var defaultSection = CommonFixture.ChooseRandom(store.Sections);

        builder.WithDefaultSection(defaultSection);

        return builder;
    }
}