using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models
{
    public class SearchBar
    {
        public SearchBar()
        {
            ResetInput();
            ResetOptions();
            Active = false;
        }

        public string Input { get; set; } = "";

        public bool Active { get; set; }
        public IEnumerable<SearchItemForShoppingListResult> Options { get; set; }

        public void ResetInput()
        {
            Input = "";
        }

        public void ResetOptions()
        {
            Options = Enumerable.Empty<SearchItemForShoppingListResult>();
        }
    }
}