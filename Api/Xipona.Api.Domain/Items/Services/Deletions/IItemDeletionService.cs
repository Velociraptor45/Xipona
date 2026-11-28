using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Services.Deletions;

public interface IItemDeletionService
{
    Task DeleteAsync(ItemId itemId);
}