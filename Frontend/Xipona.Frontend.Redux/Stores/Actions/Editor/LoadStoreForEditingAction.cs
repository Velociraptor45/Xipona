using Xipona.Frontend.Redux.Stores.States;

namespace Xipona.Frontend.Redux.Stores.Actions.Editor;
public record LoadStoreForEditingAction(Guid StoreId);
public record LoadStoreForEditingFinishedAction(EditedStore Store);