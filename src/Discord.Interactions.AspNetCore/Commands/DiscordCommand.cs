using System.Threading.Tasks;
using Discord.Interactions.AspNetCore.External;
using Discord.Interactions.AspNetCore.External.ApplicationCommand;
using Optional;

namespace Discord.Interactions.AspNetCore.Commands
{
    public abstract class DiscordCommand
    {
        public string Name { get; protected init; } = null!;
        public string Description { get; protected init; } = null!;
        public ApplicationCommandOptionType Type { get; protected init; }
        public Option<ApplicationCommandOptionChoice[]> Choices { get; protected init; }

        public abstract Task<InteractionResponse> HandleCommandAsync(Interaction interaction);


    }
}
