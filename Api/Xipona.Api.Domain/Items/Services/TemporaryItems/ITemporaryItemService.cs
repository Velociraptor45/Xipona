namespace ProjectHermes.Xipona.Api.Domain.Items.Services.TemporaryItems;

public interface ITemporaryItemService
{
    Task MakePermanentAsync(PermanentItem permanentItem);
}