using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public class Sections : IEnumerable<ISection>
{
    private readonly ISectionFactory _sectionFactory;
    private readonly IDictionary<SectionId, ISection> _sections;

    public Sections(IEnumerable<ISection> sections, ISectionFactory sectionFactory)
    {
        //todo add sorting & default section validation

        _sections = sections?.ToDictionary(s => s.Id) ?? throw new ArgumentNullException(nameof(sections));
        _sectionFactory = sectionFactory ?? throw new ArgumentNullException(nameof(sectionFactory));
    }

    public void UpdateMany(IEnumerable<SectionUpdate> updates)
    {
        var updatesList = updates.ToList();

        var sectionsToUpdate = updatesList.Where(s => s.Id.HasValue).ToDictionary(update => update.Id!.Value);
        var sectionsToCreate = updatesList.Where(s => !s.Id.HasValue);
        var sectionIdsToDelete = _sections.Keys.Where(id => !sectionsToUpdate.ContainsKey(id));
        var newSections = sectionsToCreate
            .Select(section => _sectionFactory.CreateNew(section.Name, section.SortingIndex, section.IsDefaultSection))
            .ToList();

        foreach (var sectionId in sectionIdsToDelete)
        {
            Remove(sectionId);
        }

        foreach (var section in sectionsToUpdate.Values)
        {
            Update(section);
        }

        AddMany(newSections);
    }

    public ISection GetDefaultSection()
    {
        return _sections.Values.First(s => s.IsDefaultSection);
    }

    public IReadOnlyCollection<ISection> AsReadOnly()
    {
        return _sections.Values.ToList().AsReadOnly();
    }

    public bool Contains(SectionId sectionId)
    {
        return _sections.ContainsKey(sectionId);
    }

    public IEnumerator<ISection> GetEnumerator()
    {
        return _sections.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void Remove(SectionId id)
    {
        _sections.Remove(id);
    }

    private void Update(SectionUpdate update)
    {
        if (!update.Id.HasValue)
            throw new ArgumentException("Id mustn't be null.");

        if (!_sections.TryGetValue(update.Id.Value, out var section))
        {
            throw new DomainException(new SectionNotFoundReason(update.Id.Value));
        }

        var updatedSection = section.Update(update);
        _sections[updatedSection.Id] = updatedSection;
    }

    private void AddMany(IEnumerable<ISection> sections)
    {
        foreach (var section in sections)
        {
            Add(section);
        }
    }

    private void Add(ISection section)
    {
        _sections.Add(section.Id, section);
    }
}