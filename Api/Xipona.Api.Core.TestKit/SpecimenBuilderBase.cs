using AutoFixture.Kernel;

namespace ProjectHermes.Xipona.Api.Core.TestKit
{
    public abstract class SpecimenBuilderBase<T> : ISpecimenBuilder where T : notnull
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (!MatchesType(request))
                return new NoSpecimen();

            return CreateInstance(context);
        }

        private static bool MatchesType(object request)
        {
            var t = request as Type;
            return typeof(T) == t;
        }

        protected abstract T CreateInstance(ISpecimenContext context);
    }
}