using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States.Validators;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.States.Validators;

public class TypeStoresValidator : IValidator<IReadOnlyCollection<EditedItemAvailability>>
{
    public bool Validate(IReadOnlyCollection<EditedItemAvailability> property, out string? errorMessage)
    {
        if (property.Count == 0)
        {
            errorMessage = "Add at least one store";
            return false;
        }

        errorMessage = null;
        return true;
    }
}