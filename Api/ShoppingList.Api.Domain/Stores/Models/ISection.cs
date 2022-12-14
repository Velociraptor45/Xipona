using ProjectHermes.ShoppingList.Api.Domain.Shared.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public interface ISection : ISortable
{
    SectionId Id { get; }
    SectionName Name { get; }
    bool IsDefaultSection { get; }
    bool IsDeleted { get; }

    ISection Modify(SectionModification modification);

    ISection Delete();
}