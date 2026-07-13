namespace Xipona.Api.Repositories.TestKit.Manufacturers.Entities;

public static class ManufacturerEntityMother
{
    public static ManufacturerEntityBuilder Active(ManufacturerEntityGodmother? godmother = null)
    {
        return (godmother?.GetFoundation() ?? new ManufacturerEntityBuilder())
            .WithDeleted(false);
    }
}