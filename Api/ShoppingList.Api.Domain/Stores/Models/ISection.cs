using ProjectHermes.ShoppingList.Api.Domain.Shared.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public interface ISection : ISortable
{
    SectionId Id { get; }
    SectionName Name { get; }
    bool IsDefaultSection { get; }
    bool IsDeleted { get; }

    ISection Update(SectionUpdate update);

    ISection Delete();
}