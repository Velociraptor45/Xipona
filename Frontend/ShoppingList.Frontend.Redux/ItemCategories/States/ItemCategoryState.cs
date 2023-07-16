using Fluxor;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
public record ItemCategoryState(
    ItemCategorySearch Search,
    ItemCategoryEditor Editor);

public class ItemCategoryFeatureState : Feature<ItemCategoryState>
{
    public override string GetName()
    {
        return nameof(ItemCategoryState);
    }

    protected override ItemCategoryState GetInitialState()
    {
        return new ItemCategoryState(
            new ItemCategorySearch(
                false,
                false,
                new List<ItemCategorySearchResult>()),
            new ItemCategoryEditor(
                null,
                false,
                false,
                false));
    }
}