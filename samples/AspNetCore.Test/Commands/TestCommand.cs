using System.Threading.Tasks;
using Discord.Interactions.AspNetCore.Commands;
using Discord.Interactions.AspNetCore.External;
using Discord.Interactions.AspNetCore.External.ApplicationCommand;
using Optional;

namespace AspNetCore.Test.Commands
{
    public class TestCommand : DiscordCommand
    {
        public TestCommand()
        {
            Name = "test";
            Description = "test description";
            Type = ApplicationCommandOptionType.String;
            Choices = Option.Some(new[]
            {
                new ApplicationCommandOptionChoice
                {
                    Name = "meme",
                    Value = "yeet"
                }
            });


        }

        public override Task<InteractionResponse> HandleCommandAsync(Interaction interaction)
        {
            return Task.FromResult(new InteractionResponse(InteractionResponseType.ChannelMessageWithSource)
            {
                Data = new InteractionApplicationCommandCallbackData
                {
                    Content = "meme",
                    Flags = InteractionCallbackFlag.Ephemeral
                }
            });
        }
    }
}
