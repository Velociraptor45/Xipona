using Xipona.Api.Contracts.Stores.Queries.Shared;
using Xipona.Api.Contracts.TestKit.Stores.Queries.Shared;

namespace Xipona.Api.Contracts.TestKit.Stores.Queries.Get;

public static class StoreContractMother
{
    public static StoreContractBuilder Valid()
    {
        var builder = new StoreContractBuilder();

        SectionContract[] sections =
        [
            SectionContractMother.NotDefault().Create(),
            SectionContractMother.Default().Create(),
            SectionContractMother.NotDefault().Create(),
        ];

        builder.WithSections(sections);

        return builder;
    }
}