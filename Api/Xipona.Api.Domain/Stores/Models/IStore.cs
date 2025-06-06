using Xipona.Api.Domain.Stores.Services.Modifications;

namespace Xipona.Api.Domain.Stores.Models;

public interface IStore
{
    StoreId Id { get; }
    StoreName Name { get; }
    bool IsDeleted { get; }
    IReadOnlyCollection<ISection> Sections { get; }
    DateTimeOffset CreatedAt { get; }

    void ChangeName(StoreName name);

    bool ContainsSection(SectionId sectionId);

    ISection GetDefaultSection();

    void ModifySectionsAsync(IEnumerable<SectionModification> sectionModifications);

    void Delete();
}