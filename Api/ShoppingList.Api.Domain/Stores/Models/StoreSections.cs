using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public class StoreSections : IEnumerable<IStoreSection>
{
    private readonly IList<IStoreSection> sections;

    public StoreSections(IEnumerable<IStoreSection> sections)
    {
        if (sections is null)
            throw new ArgumentNullException(nameof(sections));

        //todo add logic checks

        this.sections = sections.ToList();
    }

    public IStoreSection GetDefaultSection()
    {
        return sections.First(s => s.IsDefaultSection);
    }

    public IReadOnlyCollection<IStoreSection> AsReadOnly()
    {
        return sections.ToList().AsReadOnly();
    }

    public bool Contains(SectionId sectionId)
    {
        return sections.Any(s => s.Id == sectionId);
    }

    public IEnumerator<IStoreSection> GetEnumerator()
    {
        return sections.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}