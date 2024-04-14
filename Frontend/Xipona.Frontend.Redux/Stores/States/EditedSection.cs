using ProjectHermes.Xipona.Frontend.Redux.Shared.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Stores.States;

public record EditedSection(Guid Key, Guid Id, string Name, bool IsDefaultSection, int SortingIndex) : ISortableItem;