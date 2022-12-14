using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Modifications;

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

    public async Task ModifyManyAsync(IEnumerable<SectionModification> modifications,
        IItemModificationService itemModificationService,
        IShoppingListModificationService shoppingListModificationService)
    {
        var modificationsList = modifications.ToList();

        var sectionsToModify = modificationsList.Where(s => s.Id.HasValue).ToDictionary(modification => modification.Id!.Value);
        var sectionsToCreate = modificationsList.Where(s => !s.Id.HasValue);
        var sectionIdsToDelete = _sections.Keys.Where(id => !sectionsToModify.ContainsKey(id)).ToArray();
        var newSections = sectionsToCreate
            .Select(section => _sectionFactory.CreateNew(section.Name, section.SortingIndex, section.IsDefaultSection))
            .ToList();

        foreach (var sectionId in sectionIdsToDelete)
        {
            Delete(sectionId);
        }

        foreach (var section in sectionsToModify.Values)
        {
            Modify(section);
        }

        AddMany(newSections);

        ValidateDefaultSection();
        AsSortableCollection.ValidateSortingIndexes(GetActive());

        foreach (var sectionId in sectionIdsToDelete)
        {
            await itemModificationService.TransferToSectionAsync(sectionId, GetDefaultSection().Id);
            await shoppingListModificationService.RemoveSectionAsync(sectionId);
        }
    }

    public ISection GetDefaultSection()
    {
        return GetActive().First(s => s.IsDefaultSection);
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
        var defaultSectionCount = GetActive().Count(s => s.IsDefaultSection);
        if (defaultSectionCount > 1)
            throw new DomainException(new MultipleDefaultSectionsReason());
    }

    private void Delete(SectionId id)
    {
        if (!_sections.TryGetValue(id, out var section))
            return;

        _sections[id] = section.Delete();
    }

    private void Modify(SectionModification modification)
    {
        if (!modification.Id.HasValue)
            throw new ArgumentException("Id mustn't be null.");

        if (!_sections.TryGetValue(modification.Id.Value, out var section))
        {
            throw new DomainException(new SectionNotFoundReason(modification.Id.Value));
        }

        var modifiedSection = section.Modify(modification);
        _sections[modifiedSection.Id] = modifiedSection;
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

    private IEnumerable<ISection> GetActive()
    {
        return _sections.Values.Where(s => !s.IsDeleted);
    }
}