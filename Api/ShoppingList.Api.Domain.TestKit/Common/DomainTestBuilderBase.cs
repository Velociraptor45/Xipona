using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Core.TestKit;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.SpecimenBuilders;

namespace ShoppingList.Api.Domain.TestKit.Common
{
    public class DomainTestBuilderBase<TModel> : TestBuilderBase<TModel>
    {
        public DomainTestBuilderBase()
            : base()
        {
            Customizations.Add(new EnumSpecimenBuilder<QuantityType>());
            Customizations.Add(new TypeRelay(typeof(IStoreItemAvailability), typeof(StoreItemAvailability)));
        }
    }
}