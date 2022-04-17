using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Models.Index.Services
{
    public interface ITemporaryItemCreationService
    {
        ShoppingListItem Create(string name);
    }
}
