using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public class RecipeTags : IEnumerable<RecipeTagId>
{
    private readonly HashSet<RecipeTagId> _tags = new();

    public RecipeTags(IEnumerable<RecipeTagId> tagIds)
    {
        foreach (var tagId in tagIds)
        {
            if (_tags.Contains(tagId))
                continue;

            _tags.Add(tagId);
        }
    }

    public IReadOnlyCollection<RecipeTagId> AsReadOnly()
    {
        return _tags;
    }

    public IEnumerator<RecipeTagId> GetEnumerator()
    {
        return _tags.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}