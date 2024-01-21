using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States.Validators;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.States.Validators;

public class StoresValidator : IValidator<(ItemMode Mode, IReadOnlyCollection<EditedItemAvailability> Availabilities)>
{
    public bool Validate((ItemMode Mode, IReadOnlyCollection<EditedItemAvailability> Availabilities) property,
        out string? errorMessage)
    {
        if (property is { Mode: ItemMode.WithoutTypes, Availabilities.Count: 0 })
        {
            errorMessage = "Add at least one store";
            return false;
        }

        errorMessage = null;
        return true;
    }
}