using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.Processing;
public record ApiRequestProcessingErrorOccurredAction(IApiRequest FailedRequest);