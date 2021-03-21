using System.Collections.Generic;

namespace ShoppingList.Api.Domain.TestKit.Common.Fixtures
{
    public interface IModelFixture<TModel, TDefinition>
        where TModel : class
        where TDefinition : new()
    {
        public TModel Create(TDefinition definition);

        public TModel CreateValid()
        {
            return CreateValid(new TDefinition());
        }

        public TModel CreateValid(TDefinition baseDefinition);

        public IEnumerable<TModel> CreateManyValid(int count = 3);

        public IEnumerable<TModel> CreateManyValid(TDefinition definition, int count = 3);

        public IEnumerable<TModel> CreateManyValid(IEnumerable<TDefinition> definitions);
    }
}