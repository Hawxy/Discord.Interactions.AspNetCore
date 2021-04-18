using System;
using System.Collections.Generic;

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
        public bool? TTS { get; init; }

        public string? Content { get; init; }

        public List<object>? Embeds { get; init; }

        public object? AllowedMentions { get; init; }

        public InteractionCallbackFlag? Flags { get; init; }
    }

    [Flags]
    public enum InteractionCallbackFlag
    {
        Ephemeral = 64
    }


}
