using System.Collections.Generic;
using System.Text.Json.Serialization;
using Optional;

namespace Discord.Interactions.AspNetCore.External.ApplicationCommand
{
    public class ApplicationCommand
    {
        public Snowflake Id { get; init; }

        public Snowflake ApplicationId { get; init; }

        public string Name { get; init; } = null!;

        public string Description { get; init; } = null!;

        public List<ApplicationCommandOption> Options { get; init; } = null!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Option<bool> DefaultPermission { get; init; }
    }
}
