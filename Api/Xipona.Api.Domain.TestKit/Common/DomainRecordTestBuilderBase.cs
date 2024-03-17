namespace ProjectHermes.Xipona.Api.Domain.TestKit.Common;

public class DomainRecordTestBuilderBase<TModel> : DomainTestBuilderBase<TModel>
{
    protected readonly List<Func<TModel, TModel>> Modifiers = new();

    public override TModel Create()
    {
        var model = base.Create();
        foreach (var modifier in Modifiers)
        {
            model = modifier(model);
        }
        return model;
    }

    public override IEnumerable<TModel> CreateMany(int count)
    {
        var models = base.CreateMany(count).ToList();
        for (int i = 0; i < models.Count; i++)
        {
            foreach (var modifier in Modifiers)
            {
                models[i] = modifier(models[i]);
            }
        }
        return models;
    }
}