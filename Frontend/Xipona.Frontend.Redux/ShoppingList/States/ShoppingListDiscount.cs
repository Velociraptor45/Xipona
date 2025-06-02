namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

public record ShoppingListDiscount(Guid Id, decimal DiscountValue, ShoppingListDiscountType Type, bool IsDeleting = false);
