﻿using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.Shared.Validations;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Models;

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
        var tags = recipeTagIds.Distinct().ToList();

        if (tags.Count != 0)
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