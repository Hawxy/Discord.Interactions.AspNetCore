using System.Threading.Tasks;

namespace Discord.Interactions.AspNetCore.Http
{
    public interface IDiscordInteractionsClient
    {
        public Task CreateGuildCommand(ulong ApplicationId, ulong GuildId, CreateGuildCommand CreateGuildCommand);
    }
}
