using ProjectHermes.Xipona.Frontend.Redux.Shared.States.Validators;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.States.Validators;

public class TypesValidator : IValidator<(ItemMode Mode, IReadOnlyCollection<EditedItemType> Types)>
{
    public bool Validate((ItemMode Mode, IReadOnlyCollection<EditedItemType> Types) property, out string? errorMessage)
    {
        if (property is { Mode: ItemMode.WithTypes, Types.Count: 0 })
        {
            errorMessage = "Add at least one type";
            return false;
        }

        errorMessage = null;
        return true;
    }
}