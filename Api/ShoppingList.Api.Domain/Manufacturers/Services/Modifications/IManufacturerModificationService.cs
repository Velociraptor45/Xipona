namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Modifications;

public interface IManufacturerModificationService
{
    Task ModifyAsync(ManufacturerModification modification);
}