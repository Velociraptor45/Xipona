namespace Xipona.Api.Repositories.TestKit.Manufacturers.Entities;

public static class ManufacturerEntityMother
{
    public static ManufacturerEntityBuilder Active(ManufacturerEntityGodmother? godmother = null)
    {
        return (godmother ?? new ManufacturerEntityGodmother()).GetFoundation()
            .WithDeleted(false);
    }
}