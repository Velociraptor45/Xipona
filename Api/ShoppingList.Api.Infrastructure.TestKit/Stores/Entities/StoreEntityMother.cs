using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.Stores.Entities;
public static class StoreEntityMother
{
    public static StoreEntityBuilder Initial()
    {
        return new StoreEntityBuilder()
            .WithDeleted(false);
    }
}
