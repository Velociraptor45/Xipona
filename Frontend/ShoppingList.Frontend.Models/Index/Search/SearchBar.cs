using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Index.Search
{
    public class SearchBar
    {
        private string input = "";

        public SearchBar()
        {
            ResetInput();
            ResetOptions();
            Active = false;
        }

        public string Input
        {
            get => input;
            set
            {
                input = value;
            }
        }

        public bool Active { get; set; }
        public IEnumerable<ItemSearchResult> Options { get; set; }

        public void ResetInput()
        {
            Input = "";
        }

        public void ResetOptions()
        {
            Options = Enumerable.Empty<ItemSearchResult>();
        }
    }
}