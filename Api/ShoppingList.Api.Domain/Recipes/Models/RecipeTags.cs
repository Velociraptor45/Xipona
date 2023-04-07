using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public class RecipeTags : IEnumerable<RecipeTagId>
{
    private HashSet<RecipeTagId> _tags = new();

    public RecipeTags(IEnumerable<RecipeTagId> tagIds)
    {
        foreach (var tagId in tagIds)
        {
            if (_tags.Contains(tagId))
                continue;

            _tags.Add(tagId);
        }
    }

    public async Task ModifyAsync(IValidator validator, IEnumerable<RecipeTagId> recipeTagIds)
    {
        var tags = recipeTagIds.ToList();

        await validator.ValidateAsync(tags);

        _tags = new HashSet<RecipeTagId>(tags);
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