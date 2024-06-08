namespace ProjectHermes.Xipona.Api.Contracts.TestKit.Stores.Queries.Shared;

public static class SectionContractMother
{
    public static SectionContractBuilder Default()
    {
        return new SectionContractBuilder()
            .WithIsDefaultSection(true);
    }

    public static SectionContractBuilder NotDefault()
    {
        return new SectionContractBuilder()
            .WithIsDefaultSection(false);
    }
}