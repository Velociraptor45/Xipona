using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States.Validators;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.States.Validators;

public class StoresOrTypesValidator : IValidator<(ItemMode Mode,
    IReadOnlyCollection<EditedItemAvailability> Availabilities, IReadOnlyCollection<EditedItemType> Types)>
{
    public bool Validate((ItemMode Mode, IReadOnlyCollection<EditedItemAvailability> Availabilities,
        IReadOnlyCollection<EditedItemType> Types) property, out string? errorMessage)
    {
        if (property is { Mode: ItemMode.NotDefined, Availabilities.Count: 0, Types.Count: 0 })
        {
            errorMessage = "Select either stores or types";
            return false;
        }

        errorMessage = null;
        return true;
    }
}