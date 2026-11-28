using Xipona.Frontend.Redux.Items.States;

namespace Xipona.Frontend.Redux.Items.Actions;
public record LoadActiveStoresAction;
public record LoadActiveStoresFinishedAction(ActiveStores Stores);