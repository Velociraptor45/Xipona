namespace ProjectHermes.Xipona.Api.Repositories.TestKit.Manufacturers.Entities;

public static class ManufacturerEntityMother
{
    public static ManufacturerEntityBuilder Active()
    {
        return new ManufacturerEntityBuilder()
            .WithDeleted(false);
    }
}