namespace ShoppingList.Api.Domain.Commands.MakeTemporaryItemPermanent
{
    public class MakeTemporaryItemPermanentCommand : ICommand<bool>
    {
        public MakeTemporaryItemPermanentCommand(PermanentItem permanentItem)
        {
            PermanentItem = permanentItem ?? throw new System.ArgumentNullException(nameof(permanentItem));
        }

        public PermanentItem PermanentItem { get; }
    }
}