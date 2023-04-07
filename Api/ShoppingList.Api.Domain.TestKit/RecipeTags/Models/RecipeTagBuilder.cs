using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.RecipeTags.Models;
public class RecipeTagBuilder : DomainTestBuilderBase<RecipeTag>
{
    public RecipeTagBuilder WithId(RecipeTagId id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public RecipeTagBuilder WithName(string name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }
}