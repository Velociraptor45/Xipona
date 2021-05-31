using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Common.Contexts
{
    public class ContextFactoryBase
    {
        protected MySqlServerVersion GetVersion()
        {
            var version = Assembly.GetEntryAssembly().GetName().Version;
            return new MySqlServerVersion(new Version(version.Major, version.Minor, version.Build));
        }
    }
}