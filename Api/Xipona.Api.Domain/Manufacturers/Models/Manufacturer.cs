using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Reasons;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Modifications;

namespace ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

public class Manufacturer : AggregateRoot, IManufacturer
{
    public Manufacturer(ManufacturerId id, ManufacturerName name, bool isDeleted, DateTimeOffset createdAt)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
        CreatedAt = createdAt;
    }

    public ManufacturerId Id { get; }
    public ManufacturerName Name { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; }

    public void Delete()
    {
        IsDeleted = true;
    }

    public void Modify(ManufacturerModification modification)
    {
        if (IsDeleted)
            throw new DomainException(new CannotModifyDeletedManufacturerReason(Id));

        Name = modification.Name;
    }
}