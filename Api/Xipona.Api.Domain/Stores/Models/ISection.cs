using Xipona.Api.Domain.Shared.Models;
using Xipona.Api.Domain.Stores.Services.Modifications;

namespace Xipona.Api.Domain.Stores.Models;

public interface ISection : ISortable
{
    SectionId Id { get; }
    SectionName Name { get; }
    bool IsDefaultSection { get; }
    bool IsDeleted { get; }

    ISection Modify(SectionModification modification);

    ISection Delete();
}