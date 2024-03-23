using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Processing;
public record ApiRequestProcessingErrorOccurredAction(IApiRequest FailedRequest);