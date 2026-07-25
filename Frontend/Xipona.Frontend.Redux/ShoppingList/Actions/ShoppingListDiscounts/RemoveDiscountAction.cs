namespace Xipona.Frontend.Redux.ShoppingList.Actions.ShoppingListDiscounts;
public record RemoveDiscountAction(Guid DiscountId);
public record RemoveDiscountStartedAction(Guid DiscountId);
public record RemoveDiscountFinishedAction(Guid DiscountId);
public record RemoveDiscountFailedAction(Guid DiscountId);