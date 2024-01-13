using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States.Validators;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Stores.States.Validators;

public class StoreNameValidator : IValidator<string>
{
    public bool Validate(string property, out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(property))
        {
            errorMessage = "Name must not be empty";
            return false;
        }

        errorMessage = null;
        return true;
    }
}