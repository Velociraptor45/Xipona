using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Stores;

public class UpdateStoreCommandConverter : IToDomainConverter<UpdateStoreContract, UpdateStoreCommand>
{
    public UpdateStoreCommand ToDomain(UpdateStoreContract source)
    {
        var sections =
            source.Sections.Select(s => new SectionUpdate(
                s.Id is null ? null : new SectionId(s.Id.Value),
                new SectionName(s.Name),
                s.SortingIndex,
                s.IsDefaultSection));

        return new UpdateStoreCommand(new StoreUpdate(new StoreId(source.Id), new StoreName(source.Name), sections));
    }
}