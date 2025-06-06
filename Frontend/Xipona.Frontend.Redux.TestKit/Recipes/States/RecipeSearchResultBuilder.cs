using Xipona.Frontend.Redux.Recipes.States;
using Xipona.Frontend.TestTools;
using System;

namespace Xipona.Frontend.Redux.TestKit.Recipes.States;
public class RecipeSearchResultBuilder : TestBuilderBase<RecipeSearchResult>
{
    public RecipeSearchResultBuilder WithId(Guid id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public RecipeSearchResultBuilder WithName(string name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }
}