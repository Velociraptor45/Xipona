using Xipona.Frontend.Redux.Shared.Ports.Requests;

namespace Xipona.Frontend.Redux.ShoppingList.Actions.Processing;
public record ApiRequestProcessingErrorOccurredAction(IApiRequest FailedRequest);