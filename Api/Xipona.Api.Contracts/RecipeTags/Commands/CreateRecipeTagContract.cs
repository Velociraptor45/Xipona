namespace ProjectHermes.Xipona.Api.Contracts.RecipeTags.Commands
{
    public class CreateRecipeTagContract
    {
        public CreateRecipeTagContract(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}