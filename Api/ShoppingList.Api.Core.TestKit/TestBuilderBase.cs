using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;

namespace ShoppingList.Api.Core.TestKit
{
    public class TestBuilderBase<TModelType> : Fixture
    {
        protected void FillContructorWith<TParameter>(string parameterName, TParameter value)
        {
            this.ConstructorArgumentFor<TModelType, TParameter>(parameterName, value);
        }

        public TModelType Create()
        {
            return this.Create<TModelType>();
        }
    }
}