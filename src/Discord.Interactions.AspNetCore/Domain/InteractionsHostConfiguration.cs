namespace Discord.Interactions.AspNetCore.Domain
{
    public class InteractionsHostConfiguration
    {
        public string Token { get; set; } = null!;
        public ulong ApplicationId { get; set; }
        public string PublicKey { get; set; } = null!;
    }
}
