using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Shared;
using ProjectHermes.Xipona.Api.Contracts.TestKit.Stores.Queries.Shared;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.Stores.Queries.Get;

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