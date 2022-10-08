using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public class Sections : IEnumerable<ISection>, ISortableCollection<ISection>
{
    private readonly ISectionFactory _sectionFactory;
    private readonly IDictionary<SectionId, ISection> _sections;

    public Sections(IEnumerable<ISection> sections, ISectionFactory sectionFactory)
    {
        _sections = sections.ToDictionary(s => s.Id);
        _sectionFactory = sectionFactory;

        AsSortableCollection.ValidateSortingIndexes(_sections.Values);
        ValidateDefaultSection();
    }

    private ISortableCollection<ISection> AsSortableCollection => this;

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

    private void ValidateDefaultSection()
    {
        var defaultSectionCount = _sections.Values.Count(s => s.IsDefaultSection);
        if (defaultSectionCount > 1)
            throw new DomainException(new MultipleDefaultSectionsReason());
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

        AsSortableCollection.ValidateSortingIndexes(_sections.Values);
        ValidateDefaultSection();
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