using System.Diagnostics.CodeAnalysis;

namespace Xipona.Frontend.Redux.Shared.States.Validators;

public class NameValidator : IValidator<string?>
{
    public bool Validate(string? property, [NotNullWhen(false)] out string? errorMessage)
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