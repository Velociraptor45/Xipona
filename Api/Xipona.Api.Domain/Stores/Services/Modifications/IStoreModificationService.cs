namespace ProjectHermes.Xipona.Api.Domain.Stores.Services.Modifications;

public interface IStoreModificationService
{
    Task ModifyAsync(StoreModification update);
}