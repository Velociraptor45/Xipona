namespace ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests
{
    public interface IApiRequest
    {
        public Guid RequestId { get; }
        public string ItemName { get; }
    }
}