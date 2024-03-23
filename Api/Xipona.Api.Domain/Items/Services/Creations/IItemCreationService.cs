using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Creations;

public interface IItemCreationService
{
    Task<ItemReadModel> CreateItemWithTypesAsync(IItem item);

    Task<ItemReadModel> CreateAsync(ItemCreation creation);
}