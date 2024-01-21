using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States.Validators;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States.Validators;

public class IngredientItemCategoryValidator : IValidator<Guid>
{
    public bool Validate(Guid property, out string? errorMessage)
    {
        if (property == Guid.Empty)
        {
            errorMessage = "Please select an item category";
            return false;
        }

        errorMessage = null;
        return true;
    }
}