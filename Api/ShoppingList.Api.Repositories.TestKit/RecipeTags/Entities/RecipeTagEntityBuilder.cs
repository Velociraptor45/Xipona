using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Entities;
using System;

namespace ProjectHermes.ShoppingList.Api.Repositories.TestKit.RecipeTags.Entities;
public class RecipeTagEntityBuilder : TestBuilderBase<RecipeTag>
{
    public RecipeTagEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(p => p.Id, id);
        return this;
    }

    public RecipeTagEntityBuilder WithName(string name)
    {
        FillPropertyWith(p => p.Name, name);
        return this;
    }

    public RecipeTagEntityBuilder WithRowVersion(Byte[] rowVersion)
    {
        FillPropertyWith(p => p.RowVersion, rowVersion);
        return this;
    }
}