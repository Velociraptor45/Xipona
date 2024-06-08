namespace ProjectHermes.Xipona.Api.Repositories.TestKit.Stores.Entities;

public static class SectionEntityMother
{
    public static SectionEntityBuilder Default()
    {
        return new SectionEntityBuilder()
            .WithIsDefaultSection(true)
            .WithIsDeleted(false);
    }

    public static SectionEntityBuilder NotDefault()
    {
        return new SectionEntityBuilder()
            .WithIsDefaultSection(false)
            .WithIsDeleted(false);
    }
}