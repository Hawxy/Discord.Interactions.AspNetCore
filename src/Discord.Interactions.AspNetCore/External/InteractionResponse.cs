using System;
using System.Collections.Generic;
using Optional;

namespace Discord.Interactions.AspNetCore.External
{
    public record InteractionResponse(InteractionResponseType Type)
    {
        public InteractionApplicationCommandCallbackData? Data { get; init; }
    }

    public enum InteractionResponseType
    {
        Pong = 1,
        ChannelMessageWithSource = 4,
        DeferredChannelMessageWithSource = 5
    }

    public record InteractionApplicationCommandCallbackData
    {
        public Option<bool> TTS { get; init; }

        public Option<string> Content { get; init; }

        public Option<object> Embeds { get; init; }

        public Option<object> AllowedMentions { get; init; }

        public Option<InteractionCallbackFlag> Flags { get; init; }
    }

    [Flags]
    public enum InteractionCallbackFlag
    {
        Ephemeral = 64
    }


}
