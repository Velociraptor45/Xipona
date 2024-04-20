using ProjectHermes.Xipona.Frontend.Redux.Shared.States.Validators;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.States.Validators;

public class ItemCategoryValidator : IValidator<Guid?>
{
    public bool Validate(Guid? property, out string? errorMessage)
    {
        if (property is null)
        {
            errorMessage = "Item category must not be empty";
            return false;
        }

        errorMessage = null;
        return true;
    }
}