using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States.Validators;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.States.Validators;

public class DuplicatedStoresValidator : IValidator<IReadOnlyCollection<EditedItemAvailability>>
{
    public bool Validate(IReadOnlyCollection<EditedItemAvailability> property, out string? errorMessage)
    {
        var duplicatedStores = property
            .GroupBy(a => a.StoreId)
            .Any(g => g.Count() > 1);

        if (duplicatedStores)
        {
            errorMessage = "There are duplicated stores";
            return false;
        }

        errorMessage = null;
        return true;
    }
}