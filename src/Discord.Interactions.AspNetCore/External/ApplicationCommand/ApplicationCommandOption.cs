using System.Text.Json.Serialization;
using Optional;

namespace Discord.Interactions.AspNetCore.External.ApplicationCommand
{
    public class ApplicationCommandOption
    {
        public ApplicationCommandOptionType Type { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Option<bool> Required { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Option<ApplicationCommandOptionChoice[]> Choices { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Option<ApplicationCommandOption[]> Options { get; set; }
    }
}
