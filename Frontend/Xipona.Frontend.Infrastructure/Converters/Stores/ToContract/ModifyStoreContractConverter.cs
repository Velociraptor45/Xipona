using Xipona.Api.Contracts.Stores.Commands.ModifyStore;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Stores.States;
using System;
using System.Linq;

namespace Xipona.Frontend.Infrastructure.Converters.Stores.ToContract;

public class ModifyStoreContractConverter : IToContractConverter<EditedStore, ModifyStoreContract>
{
    public ModifyStoreContract ToContract(EditedStore store)
    {
        var sections = store.Sections.Select(s => new ModifySectionContract(
            s.Id == Guid.Empty ? null : s.Id,
            s.Name,
            s.SortingIndex,
            s.IsDefaultSection));

        return new ModifyStoreContract(
            store.Id,
            store.Name,
            sections);
    }
}