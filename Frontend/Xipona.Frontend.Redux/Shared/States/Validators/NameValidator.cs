namespace ProjectHermes.Xipona.Frontend.Redux.Shared.States.Validators;

public class NameValidator : IValidator<string?>
{
    public bool Validate(string? property, out string? errorMessage)
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