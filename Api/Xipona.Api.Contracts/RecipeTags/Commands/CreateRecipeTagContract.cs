namespace ProjectHermes.Xipona.Api.Contracts.RecipeTags.Commands
{
    /// <summary>
    /// Represents the contract for creating a recipe tag.
    /// </summary>
    public class CreateRecipeTagContract
    {
        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        public CreateRecipeTagContract(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of the recipe tag.
        /// </summary>
        public string Name { get; set; }
    }
}