using ProjectHermes.ShoppingList.Api.Core;

namespace ShoppingList.Api.Core.TestKit
{
    public abstract class GenericPrimitiveBuilderBase<TModel, TPrimitive, TBuilder> : TestBuilderBase<TModel>
        where TPrimitive : struct
        where TModel : GenericPrimitive<TPrimitive>
        where TBuilder : GenericPrimitiveBuilderBase<TModel, TPrimitive, TBuilder>
    {
        public TBuilder WithValue(TPrimitive value)
        {
            FillContructorWith("value", value);
            return (TBuilder)this;
        }
    }
}