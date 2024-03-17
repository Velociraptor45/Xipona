using ProjectHermes.Xipona.Frontend.Redux.Shared.States.Validators;

namespace ProjectHermes.Xipona.Frontend.Redux.Stores.States.Validators;

public class StoreSectionNameValidator : IValidator<string>
{
    public bool Validate(string property, out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(property))
        {
            errorMessage = "Section name must not be empty";
            return false;
        }

        errorMessage = null;
        return true;
    }
}