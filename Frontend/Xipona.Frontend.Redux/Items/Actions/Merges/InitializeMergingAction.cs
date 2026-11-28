using Xipona.Frontend.Redux.Items.States;

namespace Xipona.Frontend.Redux.Items.Actions.Merges;
public record InitializeMergingAction(IReadOnlyCollection<Guid> ItemIds);
public record InitializeMergingFinishedAction(IReadOnlyCollection<EditedItem> Items);