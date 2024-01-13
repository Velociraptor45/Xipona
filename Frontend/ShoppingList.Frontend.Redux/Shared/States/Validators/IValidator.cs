namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.States.Validators;

public interface IValidator<in TProperty>
{
    /// <summary>
    /// Returns true if the property is valid, false otherwise.
    /// </summary>
    /// <param name="property"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    bool Validate(TProperty property, out string? errorMessage);
}