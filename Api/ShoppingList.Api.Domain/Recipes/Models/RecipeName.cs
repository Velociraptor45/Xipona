using ProjectHermes.ShoppingList.Api.Domain.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
public class RecipeName : Name
{
    public RecipeName(string value) : base(value)
    {
    }
}
