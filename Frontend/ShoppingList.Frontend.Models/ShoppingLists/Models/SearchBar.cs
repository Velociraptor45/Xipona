using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models
{
    public class SearchBar
    {
        private string _input = "";

        public SearchBar()
        {
            ResetInput();
            ResetOptions();
            Active = false;
        }

        public string Input
        {
            get => _input;
            set
            {
                _input = value;
                OnInputChanged?.Invoke(_input);
            }
        }

        public bool Active { get; set; }
        public IEnumerable<SearchItemForShoppingListResult> Options { get; set; }

        public Action<string> OnInputChanged { get; set; }

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