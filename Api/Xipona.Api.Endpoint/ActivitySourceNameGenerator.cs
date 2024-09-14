namespace ProjectHermes.Xipona.Api.Endpoint;

public static class ActivitySourceNameGenerator
{
    public static string Generate<T>()
    {
        return $"{typeof(T).Namespace!}.{typeof(T).Name}";
    }
}