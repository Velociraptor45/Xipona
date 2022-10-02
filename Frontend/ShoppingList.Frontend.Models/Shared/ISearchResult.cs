using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared;
public interface ISearchResult
{
    Guid Id { get; set; }
    string Name { get; set; }
}
