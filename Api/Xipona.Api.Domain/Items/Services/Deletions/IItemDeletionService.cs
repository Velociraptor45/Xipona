using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Deletions;

public interface IItemDeletionService
{
    Task DeleteAsync(ItemId itemId);
}