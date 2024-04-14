using ProjectHermes.Xipona.Api.Core.Services;
using ProjectHermes.Xipona.Api.Domain.Stores.Services.Creations;

namespace ProjectHermes.Xipona.Api.Domain.Stores.Models.Factories;

public class StoreFactory : IStoreFactory
{
    private readonly ISectionFactory _sectionFactory;
    private readonly IDateTimeService _dateTimeService;

    public StoreFactory(ISectionFactory sectionFactory, IDateTimeService dateTimeService)
    {
        _sectionFactory = sectionFactory;
        _dateTimeService = dateTimeService;
    }

    public IStore Create(StoreId id, StoreName name, bool isDeleted, IEnumerable<ISection> sections,
        DateTimeOffset createdAt)
    {
        return new Store(id, name, isDeleted, new Sections(sections, _sectionFactory), createdAt);
    }

    public IStore CreateNew(StoreCreation creationInfo)
    {
        var sections = creationInfo.Sections.Select(s => _sectionFactory.CreateNew(s));

        return Create(new StoreId(Guid.NewGuid()), creationInfo.Name, false, sections, _dateTimeService.UtcNow);
    }
}