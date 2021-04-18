using System.Collections.Generic;
using System.Text.Json.Serialization;
using Discord.Interactions.AspNetCore.External.ApplicationCommand;
using Optional;

namespace Discord.Interactions.AspNetCore.External
{
    public record Interaction
    {
        public Snowflake Id { get; init; }

        public Snowflake ApplicationId { get; init; }

        public InteractionType Type { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Option<ApplicationCommandInteractionData> Data { get; init; }

        public Snowflake GuildId { get; init; }

        public Snowflake ChannelId { get; init; }
       
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Option<object> Member { get; init; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Option<object> User { get; init; }

        public string Token { get; init; } = null!;

        public int Version { get; init; }
    }


    public record ApplicationCommandInteractionData
    {
        public Snowflake Id { get; init; }

        public string Name { get; init; } = null!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Option<ApplicationCommandInteractionDataResolved> Resolved { get; init; }

        public List<ApplicationCommandInteractionDataOption> Options { get; init; } = null!;
    }
    //TODO proper types
    public record ApplicationCommandInteractionDataResolved
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        private Option<object> Users { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Option<object> Members { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Option<object> Roles { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Option<object> Channels { get; init; }
    }


    public record ApplicationCommandInteractionDataOption
    {
        public string Name { get; init; } = null!;

        public ApplicationCommandOptionType  Type { get; init; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Option<object> Value { get; init; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Option<List<ApplicationCommandInteractionDataOption>> Options { get; init; }
    }

    public enum InteractionType
    {
        Ping = 1,
        ApplicationCommand = 2
    }
}
